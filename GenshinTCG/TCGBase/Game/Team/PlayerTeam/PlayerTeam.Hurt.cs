namespace TCGBase
{
    public partial class PlayerTeam
    {
        /// <summary>
        /// 输入的dv满足targetRelative=false<br/>
        /// 在其中进行effect的调用
        /// </summary>
        private List<HurtSender> InnerHurtCompute(IDamageSource ds, out bool overload, DamageVariable start)
        {
            Queue<DamageVariable> queue = new();
            List<HurtSender> hss = new();
            overload = false;
            queue.Enqueue(start);
            while (queue.Any())
            {
                var dv = queue.Dequeue();
                if (dv.TargetExcept)
                {
                    for (int i = 0; i < Characters.Length; i++)
                    {
                        int index = (i + CurrCharacter) % Characters.Length;
                        if (index != dv.TargetIndex && Characters[index].Alive)
                        {
                            queue.Enqueue(new(dv.DirectSource, dv.Element, dv.Damage, index, false, dv.DamageTargetCategory));
                        }
                    }
                }
                else
                {
                    //only one target
                    //TODO:附魔应该在这里吗？？
                    //TODO:prehurtsender的teamindex?
                    RealGame.InstantTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.ElementEnchant), dv);
                    int initialelement = GetDamageReaction(dv);
                    if (dv.Element != -1)
                    {
                        if (dv.DamageTargetCategory == DamageTargetCategory.Enemy)
                        {
                            RealGame.InstantTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.DamageIncrease, initialelement), dv);
                            RealGame.InstantTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.DamageMul, initialelement), dv);
                        }
                        else
                        {
                            InstantTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.DamageIncrease, initialelement), dv);
                            InstantTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.DamageMul, initialelement), dv);
                        }
                        InstantTrigger(new PreHurtSender(TeamIndex, ds, SenderTag.HurtDecrease, initialelement), dv);
                    }
                    overload |= ReactionItemGenerate(dv.TargetIndex, dv.Reaction, ds, initialelement);
                    hss.Add(new(TeamIndex, dv, dv.Reaction, dv.DirectSource, ds, initialelement));
                }
                if (dv.SubDamage != null)
                {
                    queue.Enqueue(dv.SubDamage);
                }
            }
            return hss;
        }
        private void TestTriggerForDMG(List<HurtSender> hss, List<int> predie_indexs, List<EmptyTeam> predie_teams)
        {
            Action? die = null;
            foreach (var hs in hss)
            {
                var index = predie_indexs.FindIndex(i => i == hs.TargetIndex);
                if (index >= 0)
                {
                    die += () =>
                    {
                        Character target = Characters[hs.TargetIndex];
                        if (target.Predie)
                        {
                            //EffectTriggerWithoutCharacter(null, hs);
                            //Enemy.EffectTrigger(hs, null);

                            predie_teams[index].EffectTrigger(hs);
                            RealGame.EffectTrigger(hs);

                            //TODO:条件队伍反转？
                            if (hs.Deadly && target.Predie)
                            {
                                //TODO:到这里的原本team角色已经被击倒，正在结算emptyteam角色
                                target.Predie = false;
                                target.Alive = false;
                                //TODO:弃置装备牌
                                RealGame.NetEventRecords.Last().Add(new DieRecord(TeamIndex, target));
                                RealGame.EffectTrigger(new DieSender(TeamIndex, hs.TargetIndex), null);
                            }
                        }
                    };
                }
                else
                {
                    RealGame.EffectTrigger(hs);
                }
            }
            die?.Invoke();
        }
        /// <summary>
        /// specialAction:特殊效果，触发在反应效果后，免于被击倒前；会放到queue里<br/>
        /// seperateAction:分隔效果，用作获得充能，触发之后，关闭queue
        /// </summary>
        private void InnerHurt(DamageVariable? dv, IDamageSource ds, Action? specialAction = null, Action? seperateAction = null)
        {
            List<HurtSender> hss = new();
            bool overload = false;

            List<int> predies = new();
            List<EmptyTeam> predie_teams = new();

            List<Persistent<AbstractCardPersistent>> antidie_effects = new();

            Queue<Action> queue = new();

            RealGame.TempDelayedTriggerQueue = queue;

            if (dv != null)
            {
                dv.ToAbsoluteIndex(CurrCharacter, Characters.Length);
                hss = InnerHurtCompute(ds, out overload, dv);
            }

            foreach (var hs in hss)
            {
                var c = Characters[hs.TargetIndex];
                if (c.HP > 0)
                {
                    c.HP -= hs.Damage;
                    if (c.HP == 0)
                    {
                        if (c.Effects.TryFind(e => e.Card.Tags.Contains(PersistentTag.AntiDie.ToString()), out var p) )
                        {
                            //TODO:check it?  && p.Card.TriggerList.ContainsKey(SenderTag.PreDie.ToString())
                            antidie_effects.Add(p);
                        }
                        else
                        {
                            c.Predie = true;
                            hs.Deadly = true;
                            predies.Add(hs.TargetIndex);
                            predie_teams.Add(ToEmpty(c));
                        }
                    }
                }
                RealGame.BroadCast(ClientUpdateCreate.CharacterUpdate.HurtUpdate(TeamIndex, hs.TargetIndex, hs.Element, hs.Damage));
            }

            if (overload)
            {
                TrySwitchToIndex(1, true);
            }
            specialAction?.Invoke();
            foreach (var die_effect in antidie_effects)
            {
                if (die_effect.Card.TriggerList.TryGetValue(SenderTag.PreDie.ToString(), out var h))
                {
                    h.Trigger(this, die_effect, new SimpleSender(TeamIndex, SenderTag.PreDie), null);
                }
            }
            //TODO: gain MP
            RealGame.TempDelayedTriggerQueue = null;
            while (queue.TryDequeue(out var trigger))
            {
                trigger.Invoke();
            }
            TestTriggerForDMG(hss, predies, predie_teams);
            if (!Characters[CurrCharacter].Alive)
            {
                if (Characters.All(p => !p.Alive))
                {
                    throw new GameOverException();
                }
                if (!Enemy.Characters[Enemy.CurrCharacter].Alive)
                {
                    throw new Exception("共死其实还没做.....");
                    //双方出战角色都被击倒 进入选择角色出战
                    var t0 = new Task(() => RealGame.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced));
                    var t1 = new Task(() => RealGame.RequestAndHandleEvent(1 - TeamIndex, 30000, ActionType.SwitchForced));
                    t0.Start();
                    t1.Start();
                    Task.WaitAll(t0, t1);
                    //TODO:其实不该执行，应该都选择出战之后才能执行...
                }
                else
                {
                    RealGame.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced);
                }
            }
        }
        /// <summary>
        /// 根据DamageVariable.DamageTargetCategory,对<b>我方队伍</b>或<b>对方队伍</b>造成伤害<br/>
        /// 对我方队伍造成伤害时，不能吃到对方队伍的增伤
        /// </summary>
        public override void DoDamage(DamageVariable? dv, IDamageSource ds, Action? action = null, Action? seperateAction = null)
            => (dv != null && dv.DamageTargetCategory == DamageTargetCategory.Enemy ? Enemy : this).InnerHurt(dv, ds, action);

        //public void DoDamage(DamageVariable? dv, IDamageSource ds, Action<PlayerTeam> action)
        //        => (dv != null && dv.DamageTargetCategory == DamageTargetCategory.Enemy ? Enemy : this).InnerHurt(dv, ds, action);
    }
}
