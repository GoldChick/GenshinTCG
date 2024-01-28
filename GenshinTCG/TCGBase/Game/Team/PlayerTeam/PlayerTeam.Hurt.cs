namespace TCGBase
{
    public partial class PlayerTeam
    {
        /// <summary>
        /// 输入的dv需要是targetRelative=false<br/>
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
                            queue.Enqueue(new(dv.DirectSource, dv.Element, dv.Damage, index, false));
                        }
                    }
                }
                else
                {
                    //only one target
                    RealGame.EffectTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.ElementEnchant), dv);
                    int initialelement = GetDamageReaction(dv);
                    if (dv.Element != -1)
                    {
                        RealGame.EffectTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.DamageIncrease, initialelement), dv);
                        RealGame.EffectTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.DamageMul, initialelement), dv);
                        EffectTrigger(new PreHurtSender(TeamIndex, ds, SenderTag.HurtMul, initialelement), dv);
                        EffectTrigger(new PreHurtSender(TeamIndex, ds, SenderTag.HurtDecrease, initialelement), dv);
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
            List<HurtSender> predies = new();
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

                            if (hs.Deadly && target.Predie)
                            {
                                //TODO:到这里的原本team角色已经被击倒，正在结算emptyteam角色
                                target.Predie = false;

                                //TODO:弃置装备牌
                                target.MP = 0;
                                target.Effects.Clear();
                                RealGame.NetEventRecords.Last().Add(new DieRecord(TeamIndex, target));
                                RealGame.EffectTrigger(new DieSender(TeamIndex, hs.TargetIndex), null);
                            }
                        }
                    };
                    predies.Add(hs);
                }
                else
                {
                    RealGame.EffectTrigger(hs, null);
                }
            }
            die?.Invoke();
        }
        private void TestTriggerForDMG(List<HurtSender> hss, List<int> predie_indexs)
        {
            List<HurtSender> predies = new();
            foreach (var hs in hss)
            {
                if (predie_indexs.Contains(hs.TargetIndex))
                {
                    predies.Add(hs);
                }
                else
                {
                    RealGame.EffectTrigger(hs, null);
                }
            }
            foreach (var hs in predies)
            {
                Character target = Characters[hs.TargetIndex];
                if (target.Predie && predie_indexs.Contains(hs.TargetIndex))
                {
                    EffectTriggerWithoutCharacter(null, hs);
                    Enemy.EffectTrigger(hs, null);
                    if (hs.Deadly && target.Predie)
                    {
                        target.Predie = false;

                        //TODO:弃置装备牌
                        target.MP = 0;
                        target.Effects.Clear();
                        RealGame.NetEventRecords.Last().Add(new DieRecord(TeamIndex, target));
                        RealGame.EffectTrigger(new DieSender(TeamIndex, hs.TargetIndex), null);
                    }
                }
            }
        }
        private void InnerHurt(DamageVariable dv, IDamageSource ds, Action? action = null, bool fromEnemy = true)
        {
            dv.ToAbsoluteIndex(CurrCharacter, Characters.Length);

            List<HurtSender> hss = InnerHurtCompute(ds, out bool overload, dv);

            List<int> predies = new();
            List<EmptyTeam> predie_teams = new();
            List<Persistent<AbstractCardPersistent>> antidie_effects = new();
            foreach (var hs in hss)
            {
                var c = Characters[hs.TargetIndex];
                if (c.HP > 0)
                {
                    c.HP -= hs.Damage;
                    if (c.HP == 0)
                    {
                        if (c.Effects.TryFind(e => e.Card.Tags.Contains(PersistentTag.AntiDie.ToString()), out var p) && p.Card.TriggerDic.ContainsKey(SenderTag.PreDie.ToString()))
                        {
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
            RealGame.InstantTrigger = false;
            if (overload)
            {
                TrySwitchToIndex(1, true);
            }
            action?.Invoke();
            foreach (var die_effect in antidie_effects)
            {
                if (die_effect.Card.TriggerDic.TryGetValue(SenderTag.PreDie.ToString(), out var h))
                {
                    h.Trigger(this, die_effect, new SimpleSender(TeamIndex, SenderTag.PreDie), null);
                }
            }
            //TODO: gain MP
            RealGame.InstantTrigger = true;
            //暂时放在这里来触发白术护盾
            RealGame.EffectTrigger(new SimpleSender(SenderTag.AfterHitLanded));

            while (RealGame.DelayedTriggerQueue.TryDequeue(out var trigger))
            {
                trigger.Invoke();
            }
            TestTriggerForDMG(hss, predies, predie_teams);
            TestTriggerForDMG(hss, predies);
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
        /// 对<b>己方队伍</b>造成伤害<br/>
        /// 不会被对方队伍的增伤影响
        /// </summary>
        /// <param name="action">伤害结算后，死亡结算前结算的东西</param>
        public override void Hurt(DamageVariable dv, IDamageSource ds, Action? action = null) => InnerHurt(dv, ds, action, false);
        /// <summary>
        /// 对<b>对方队伍</b>造成伤害
        /// </summary>
        public override void DoDamage(DamageVariable dv, IDamageSource ds, Action? action = null) => Enemy.InnerHurt(dv, ds, action);
    }
}
