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
                    Game.EffectTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.ElementEnchant), dv);
                    int initialelement = GetDamageReaction(dv);
                    if (dv.Element != -1)
                    {
                        Game.EffectTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.DamageIncrease, initialelement), dv);
                        Game.EffectTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.DamageMul, initialelement), dv);
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

        private void InnerHurt(DamageVariable dv, IDamageSource ds, Action? action = null, bool fromEnemy = true)
        {
            dv.ToAbsoluteIndex(CurrCharacter, Characters.Length);

            List<HurtSender> hss = InnerHurtCompute(ds, out bool overload, dv);
            foreach (var hs in hss)
            {
                Characters[hs.TargetIndex].HP -= hs.Damage;
                Game.BroadCast(ClientUpdateCreate.CharacterUpdate.HurtUpdate(TeamIndex, hs.TargetIndex, hs.Element, hs.Damage));
            }
            if (overload)
            {
                TrySwitchToIndex(1, true);
            }
            action?.Invoke();

            foreach (var hs in hss)
            {
                Game.EffectTrigger(hs, null);
            }
            //check die ...
            for (int i = 0; i < Characters.Length; i++)
            {
                int curr = (i + CurrCharacter) % Characters.Length;
                Character target = Characters[curr];
                if (target.Predie)
                {
                    Game.EffectTrigger(new DieSender(TeamIndex, curr), null);
                    target.Predie = false;

                    target.MP = 0;
                    target.Effects.Clear();
                    //TODO:弃置装备牌
                    Game.NetEventRecords.Last().Add(new DieRecord(TeamIndex, target));
                }
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
                    //双方出战角色都被击倒 进入选择角色出战
                    var t0 = new Task(() => Game.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced));
                    var t1 = new Task(() => Game.RequestAndHandleEvent(1 - TeamIndex, 30000, ActionType.SwitchForced));
                    t0.Start();
                    t1.Start();
                    Task.WaitAll(t0, t1);
                    //TODO:其实不该执行，应该都选择出战之后才能执行...
                }
                else
                {
                    Game.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced);
                }
            }
        }
        /// <summary>
        /// 对<b>己方队伍</b>造成伤害<br/>
        /// 不会被对方队伍的增伤影响
        /// </summary>
        /// <param name="action">伤害结算后，死亡结算前结算的东西</param>
        public void Hurt(DamageVariable dv, IDamageSource ds, Action? action = null) => InnerHurt(dv, ds, action, false);
        /// <summary>
        /// 对<b>对方队伍</b>造成伤害
        /// </summary>
        public void DoDamage(DamageVariable dv, IDamageSource ds, Action? action = null) => Enemy.InnerHurt(dv, ds, action);
    }
}
