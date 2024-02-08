namespace TCGBase
{
    public partial class PlayerTeam
    {
        /// <summary>
        /// 输入的dv满足targetRelative=false<br/>
        /// 在其中进行effect的调用
        /// </summary>
        private List<HurtSender> InnerHurtCompute(ITriggerable triggerable, out bool overload, DamageVariable start)
        {
            Queue<DamageVariable> queue = new();
            List<HurtSender> hss = new();
            overload = false;
            queue.Enqueue(start);
            while (queue.Any())
            {
                var dv = queue.Dequeue();
                if (dv.TargetArea == DamageTargetArea.TargetExcept)
                {
                    for (int i = 0; i < Characters.Length; i++)
                    {
                        int index = (i + CurrCharacter) % Characters.Length;
                        if (index != dv.TargetIndex && Characters[index].Alive)
                        {
                            queue.Enqueue(new(dv.DirectSource, dv.Element, dv.Damage, index, DamageTargetArea.TargetOnly, dv.DamageTargetTeam));
                        }
                    }
                }
                else
                {
                    //only one target
                    //TODO:附魔应该在这里吗？？
                    //TODO:prehurtsender的teamindex?
                    Game.InstantTrigger(new PreHurtSender(1 - TeamIndex, triggerable, SenderTag.ElementEnchant), dv);
                    int initialelement = GetDamageReaction(dv);
                    if (dv.Element is not DamageElement.Pierce)
                    {
                        if (dv.DamageTargetTeam == DamageTargetTeam.Enemy)
                        {
                            Game.InstantTrigger(new PreHurtSender(1 - TeamIndex, triggerable, SenderTag.DamageIncrease, initialelement), dv);
                            Game.InstantTrigger(new PreHurtSender(1 - TeamIndex, triggerable, SenderTag.DamageMul, initialelement), dv);
                        }
                        else
                        {
                            InstantTrigger(new PreHurtSender(1 - TeamIndex, triggerable, SenderTag.DamageIncrease, initialelement), dv);
                            InstantTrigger(new PreHurtSender(1 - TeamIndex, triggerable, SenderTag.DamageMul, initialelement), dv);
                        }
                        InstantTrigger(new PreHurtSender(TeamIndex, triggerable, SenderTag.HurtDecrease, initialelement), dv);
                    }
                    overload |= ReactionItemGenerate(dv.TargetIndex, dv.Reaction, triggerable, initialelement);
                    hss.Add(new(TeamIndex, dv, dv.Reaction, dv.DirectSource, triggerable, initialelement));
                }
                if (dv.SubDamage != null)
                {
                    queue.Enqueue(dv.SubDamage);
                }
            }
            return hss;
        }
        /// <summary>
        /// specialAction:特殊效果，触发在反应效果后，免于被击倒前；会放到queue里<br/>
        /// </summary>
        private void InnerHurt(DamageVariable? dv, ITriggerable triggerable, Action? specialAction = null)
        {
            bool overload = false;

            List<Persistent> antidie_effects = new();
            List<HurtSender> valid_hss = new();
            if (dv != null)
            {
                dv.ToAbsoluteIndex(CurrCharacter, Characters.Length);
                foreach (var hs in InnerHurtCompute(triggerable, out overload, dv))
                {
                    var c = Characters[hs.TargetIndex];
                    if (c.HP > 0)
                    {
                        valid_hss.Add(hs);
                        c.HP -= hs.Damage;
                        if (c.HP == 0)
                        {
                            if (c.Effects.TryFind(e => e.CardBase.Tags.Contains(CardTag.AntiDie.ToString()), out var p) && p.CardBase.TriggerableList.ContainsKey(SenderTag.PreDie.ToString()))
                            {
                                antidie_effects.Add(p);
                            }
                            else
                            {
                                c.ToDieLimbo();
                            }
                        }
                    }
                    Game.BroadCast(ClientUpdateCreate.CharacterUpdate.HurtUpdate(TeamIndex, hs.TargetIndex, hs.Element, hs.Damage));
                }
            }
            if (overload)
            {
                TrySwitchToIndex(1, true);
            }
            specialAction?.Invoke();
            foreach (var die_effect in antidie_effects)
            {
                foreach (var it in die_effect.CardBase.TriggerableList)
                {
                    if (it.Tag == SenderTag.PreDie.ToString())
                    {
                        it.Trigger(this, die_effect, new SimpleSender(TeamIndex, SenderTag.PreDie), null);
                    }
                }
            }
            foreach (var c in Characters)
            {
                if (c.HP == 0 && c.Alive)
                {
                    c.ToDie();
                }
            }

            var alive_hss = valid_hss.Where(hs => Characters[hs.TargetIndex].Alive);
            var died_hss = valid_hss.Where(hs => !Characters[hs.TargetIndex].Alive);

            Game.TempDelayedTriggerQueue?.Enqueue(() =>
            {
                foreach (var hs in alive_hss)
                {
                    Game.EffectTrigger(hs);
                }
                foreach (var hs in died_hss)
                {
                    Characters[hs.TargetIndex].DieTrigger(hs);
                    Game.NetEventRecords.Last().Add(new DieRecord(TeamIndex, Characters[hs.TargetIndex]));
                    Game.EffectTrigger(hs);
                }
                if (!Characters[CurrCharacter].Alive)
                {
                    if (Characters.All(p => !p.Alive))
                    {
                        throw new GameOverException();
                    }
                    if (!Enemy.Characters[Enemy.CurrCharacter].Alive)
                    {
                        throw new Exception("共死其实还没做.....");
                        ////双方出战角色都被击倒 进入选择角色出战
                        //var t0 = new Task(() => RealGame.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced));
                        //var t1 = new Task(() => RealGame.RequestAndHandleEvent(1 - TeamIndex, 30000, ActionType.SwitchForced));
                        //t0.Start();
                        //t1.Start();
                        //Task.WaitAll(t0, t1);
                        ////TODO:其实不该执行，应该都选择出战之后才能执行...
                    }
                    else
                    {
                        Game.RequestAndHandleEvent(TeamIndex, 30000, OperationType.Switch);
                    }
                }
            });
        }
        /// <summary>
        /// 根据DamageVariable.DamageTargetCategory,对<b>我方队伍</b>或<b>对方队伍</b>造成伤害<br/>
        /// 对我方队伍造成伤害时，不能吃到对方队伍的增伤
        /// </summary>
        public void DoDamage(DamageVariable? dv, ITriggerable triggerable, Action? action = null)
            => (dv != null && dv.DamageTargetTeam == DamageTargetTeam.Enemy ? Enemy : this).InnerHurt(dv, triggerable, action);

        //public void DoDamage(DamageVariable? dv, IDamageSource ds, Action<PlayerTeam> action)
        //        => (dv != null && dv.DamageTargetCategory == DamageTargetCategory.Enemy ? Enemy : this).InnerHurt(dv, ds, action);
    }
}
