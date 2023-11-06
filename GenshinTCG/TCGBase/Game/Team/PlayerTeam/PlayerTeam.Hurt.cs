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
                        DamageVariable dv_person = new(d.DirectSource, d.Element, d.Damage, index);
                        hss.AddRange(Hurt(ds, out bool one_overload, dv_person));
                        overload = overload || one_overload;
                    }
                }
            }
            else
            {
                //only one target
                Game.EffectTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.ElementEnchant), d);
                GetDamageReaction(d, out var mul);
                if (d.Element != -1)
                {
                    Game.EffectTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.DamageIncrease), d);
                    Game.EffectTrigger(new PreHurtSender(TeamIndex, ds, SenderTag.HurtDecrease), d);
                    Game.EffectTrigger(new PreHurtSender(1 - TeamIndex, ds, SenderTag.DamageMul), d);
                    Game.EffectTrigger(new PreHurtSender(TeamIndex, ds, SenderTag.HurtMul), d);
                }

                hss.Add(new(TeamIndex, d, d.Reaction));
                //生成effect等
                overload = ReactionTrigger(d.TargetIndex,d.Reaction);

                if (mul != null)
                {
                    hss.AddRange(Hurt(ds, out bool one_overload, mul));
                    overload = overload || one_overload;
                }
            }
            return hss;
        }
        /// <returns>经过merge的hurtsender们</returns>
        private List<HurtSender> MultiHurt(IDamageSource ds, out bool overload, params DamageVariable[] dvs)
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
                    EffectTrigger(Game, TeamIndex, new DieSender(TeamIndex, curr), null);
                    target.Predie = false;
                    target.Alive = false;

                    target.MP = 0;
                    target.Weapon.Clear();
                    target.Artifact.Clear();
                    target.Talent.Clear();
                    target.Effects.Clear();
                }
            }
            if (Characters.All(p => !p.Alive))
            {
                throw new GameOverException();
            }
            if (!Characters[CurrCharacter].Alive)
            {
                Game.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced, "Character Died");
            }
        }
        /// <param name="action">伤害结算后，死亡结算前结算的东西，如[风压剑]</param>
        public void MultiHurt(DamageVariable[] dvs, IDamageSource ds, Action? action = null)
        {
            //assert dvs.all(p=>p.targetrelative)
            DamageVariable[] dvs_person = dvs.Select(p => new DamageVariable(ds.DamageSource, p.Element, p.Damage, (p.TargetIndex + CurrCharacter) % Characters.Length, p.TargetExcept)).ToArray();
            List<HurtSender> hss = MultiHurt(ds, out bool overload, dvs_person);
            foreach (var hs in hss)
            {
                Characters[hs.TargetIndex].HP -= hs.Damage;
            }
            for (int i = 0; i < Characters.Length; i++)
            {
                int curr = (i + CurrCharacter) % Characters.Length;
                var cha = Characters[curr];
                if (cha.HP == 0 && cha.Alive && !cha.Predie)
                {
                    EffectTrigger(Game, TeamIndex, new DieSender(TeamIndex, curr, true), null);
                    if (cha.HP == 0)
                    {
                        cha.Predie = true;
                    }
                }
            }
            if (Characters.All(p => p.HP == 0))
            {
                throw new GameOverException();
            }
            //判断共死
            if (Characters[CurrCharacter].HP == 0 && Enemy.Characters[Enemy.CurrCharacter].HP == 0)
            {
                //双方出战角色都被击倒 进入选择角色出战
                var t0 = new Task(() => Game.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced, "Die Together"));
                var t1 = new Task(() => Game.RequestAndHandleEvent(1 - TeamIndex, 30000, ActionType.SwitchForced, "Die Together"));
                t0.Start();
                t1.Start();
                Task.WaitAll(t0, t1);
            }
            action?.Invoke();
            if (overload)
            {
                SwitchToNext();
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
