namespace TCGBase
{
    public partial class PlayerTeam
    {
        /// <summary>
        /// 输入的dv需要是targetRelative=false<br/>
        /// 在其中进行effect的调用
        /// </summary>
        /// <param name="d">targetRelative=false</param>
        /// <param name="overload">是否有出战角色被超载</param>
        private List<HurtSender> Hurt(IDamageSource ds, out bool overload, DamageVariable d)
        {
            List<HurtSender> hss = new();
            overload = false;

            if (d.TargetExcept)
            {
                //except to non-except
                for (int i = 0; i < Characters.Length; i++)
                {
                    int index = (i + CurrCharacter) % Characters.Length;
                    if (index != d.TargetIndex)
                    {
                        DamageVariable dv_person = new(d.DirectSource, d.Element, d.Damage, index, false, d.SubDamage);
                        hss.AddRange(Hurt(ds, out bool one_overload, dv_person));
                        overload = overload || one_overload;
                    }
                }
            }
            else
            {
                //only one target
                Game.EffectTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.ElementEnchant), d);
                int initialelement = GetDamageReaction(d);
                if (d.Element != -1)
                {
                    Game.EffectTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.DamageIncrease, initialelement), d);
                    Game.EffectTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.DamageMul, initialelement), d);
                    EffectTrigger(new PreHurtSender(TeamIndex, ds, SenderTag.HurtMul, initialelement), d, false);
                    EffectTrigger(new PreHurtSender(TeamIndex, ds, SenderTag.HurtDecrease, initialelement), d, false);
                }

                hss.Add(new(TeamIndex, d, d.Reaction, d.DirectSource, ds, initialelement));
                //生成effect等
                overload = ReactionItemGenerate(d.TargetIndex, d.Reaction, ds, initialelement);
            }

            if (d.SubDamage != null)
            {
                if (d.SubDamage.TargetRelative)
                {
                    d.SubDamage.ToAbsoluteIndex(CurrCharacter, Characters.Length);
                }
                hss.AddRange(Hurt(ds, out bool one_overload, d.SubDamage));
                overload = overload || one_overload;
            }
            return hss;
        }
        /// <returns>经过merge的hurtsender们</returns>
        private List<HurtSender> MultiHurt(IDamageSource ds, out bool overload, IEnumerable<DamageVariable> dvs)
        {
            overload = false;
            List<HurtSender> hss = new();
            foreach (var item in dvs)
            {
                hss.AddRange(Hurt(ds, out bool one_overload, item));
                overload = overload || one_overload;
            }
            List<HurtSender> selects = new();
            foreach (var hs in hss)
            {
                var hurt = selects.Find(h => h.TargetIndex == hs.TargetIndex && h.Element == hs.Element);
                if (hurt != null)
                {
                    hurt.Damage += hs.Damage;
                }
                else
                {
                    selects.Add(hs);
                }
            }
            return selects;
        }
        /// <summary>
        /// 确定死亡的角色
        /// </summary>
        private void CheckDie()
        {
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
                    Game.Records.Last().Add(new DieRecord(TeamIndex, target));
                }
            }
            if (Characters.All(p => !p.Alive))
            {
                throw new GameOverException();
            }
            if (!Characters[CurrCharacter].Alive)
            {
                Game.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced);
            }
        }
        /// <param name="action">伤害结算后，死亡结算前结算的东西，如[风压剑]</param>
        public void MultiHurt(DamageVariable[] dvs, IDamageSource ds, Action? action = null)
        {
            List<DamageVariable> dvs_person = dvs.ToList();
            dvs_person.ForEach(d => { d.ToSource(ds.DamageSource); d.ToAbsoluteIndex(CurrCharacter, Characters.Length); });

            List<HurtSender> hss = MultiHurt(ds, out bool overload, dvs_person);
            foreach (var hs in hss)
            {
                Characters[hs.TargetIndex].HP -= hs.Damage;
                Game.BroadCast(ClientUpdateCreate.CharacterUpdate.HurtUpdate(TeamIndex, hs.TargetIndex, hs.Element, hs.Damage));
            }
            if (overload)
            {
                SwitchToNext();
            }
            action?.Invoke();
            for (int i = 0; i < Characters.Length; i++)
            {
                int curr = (i + CurrCharacter) % Characters.Length;
                var cha = Characters[curr];
                if (cha.HP == 0 && cha.Alive)
                {
                    EffectTrigger(new DieSender(TeamIndex, curr, true), null, false);
                    if (cha.HP == 0)
                    {
                        cha.Predie = true;
                        cha.Alive = false;
                        cha.Element = 0;
                    }
                }
            }
            Game.EffectUpdate();

            if (Characters.All(p => p.HP == 0))
            {
                throw new GameOverException();
            }
            //判断共死
            if (Characters[CurrCharacter].HP == 0 && Enemy.Characters[Enemy.CurrCharacter].HP == 0)
            {
                //双方出战角色都被击倒 进入选择角色出战
                var t0 = new Task(() => Game.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced));
                var t1 = new Task(() => Game.RequestAndHandleEvent(1 - TeamIndex, 30000, ActionType.SwitchForced));
                t0.Start();
                t1.Start();
                Task.WaitAll(t0, t1);
            }
            foreach (var hs in hss)
            {
                Game.EffectTrigger(hs, null);
            }
            CheckDie();
        }
        /// <param name="action">伤害结算后，死亡结算前结算的东西</param>
        public void Hurt(DamageVariable dv, IDamageSource ds, Action? action = null) => MultiHurt(new DamageVariable[] { dv }, ds, action);
    }
}
