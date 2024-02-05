namespace TCGBase
{
    public partial class PlayerTeam
    {
        /// <summary>
        /// 输入的dv需要绝对坐标<br/>
        /// split出的healsender都为alive的角色上的
        /// </summary>
        private List<HealSender> SplitHeal(ITriggerable triggerable, HealVariable d)
        {
            List<HealSender> hss = new();
            if (d.TargetExcept)
            {
                //except to non-except
                for (int i = 0; i < Characters.Length; i++)
                {
                    int index = (i + CurrCharacter) % Characters.Length;
                    if (index != d.TargetIndex)
                    {
                        hss.AddRange(SplitHeal(triggerable, new(d.DirectSource, d.Amount, index)));
                    }
                }
            }
            else if (Characters[d.TargetIndex].Alive)
            {
                // PreHeal?
                // not necessary now

                //Game.EffectTrigger(new PreHurSender(1 - TeamIndex, ds, SenderTag.ElementEnchant), d);
                hss.Add(new(TeamIndex, d.Amount, d.TargetIndex));
            }
            return hss;
        }
        private List<HealSender> MergeHeal(ITriggerable triggerable, params HealVariable[] dvs_person)
        {
            List<HealSender> hss = new();
            foreach (var item in dvs_person)
            {
                hss.AddRange(SplitHeal(triggerable, item));
            }
            int l = Characters.Length;
            hss.Sort((hs1, hs2) => (hs1.TargetIndex + l - CurrCharacter) % l - (hs2.TargetIndex + l - CurrCharacter) % l);
            return hss.GroupBy(hs => hs.TargetIndex).Select(hsg => new HealSender(TeamIndex, hsg.Sum(hs => hs.Amount), hsg.Key)).ToList();
        }
        /// <summary>
        /// 为了偷懒，使用了DamageVariable作为了治疗<br/>
        /// 仅TargetIndex TargetExcept Damage有效
        /// </summary>
        public override void Heal(ITriggerable triggerable, params HealVariable[] dvs)
        {
            HealVariable[] dvs_person = dvs.Select(p => new HealVariable(p.Amount, (p.TargetIndex + CurrCharacter) % Characters.Length, p.TargetExcept)).ToArray();
            List<HealSender> hss = MergeHeal(triggerable, dvs_person);
            foreach (var hs in hss)
            {
                var c = Characters[hs.TargetIndex];
                hs.Amount = int.Min(c.CharacterCard.MaxHP - c.HP, hs.Amount);
                c.HP += hs.Amount;
                RealGame.BroadCast(ClientUpdateCreate.CharacterUpdate.HealUpdate(TeamIndex, hs.TargetIndex, hs.Amount));
            }
            foreach (var hs in hss)
            {
                RealGame.EffectTrigger(hs, null);
            }
        }
    }
}
