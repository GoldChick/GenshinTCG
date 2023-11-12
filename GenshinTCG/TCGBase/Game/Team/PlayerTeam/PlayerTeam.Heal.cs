namespace TCGBase
{
    public partial class PlayerTeam
    {
        /// <summary>
        /// 输入的dv需要绝对坐标<br/>
        /// split出的healsender都为alive的角色上的
        /// </summary>
        private List<HealSender> SplitHeal(IDamageSource ds, DamageVariable d)
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
                        hss.AddRange(SplitHeal(ds, new(d.DirectSource, d.Element, d.Damage, index)));
                    }
                }
            }
            else if (Characters[d.TargetIndex].Alive)
            {
                // PreHeal?
                // not necessary now

                //Game.EffectTrigger(new PreHurSender(1 - TeamIndex, ds, SenderTag.ElementEnchant), d);
                hss.Add(new(TeamIndex, d));
            }
            return hss;
        }
        private List<HealSender> MergeHeal(IDamageSource ds, params DamageVariable[] dvs_person)
        {
            List<HealSender> hss = new();
            foreach (var item in dvs_person)
            {
                hss.AddRange(SplitHeal(ds, item));
            }
            int l = Characters.Length;
            hss.Sort((hs1, hs2) => (hs1.TargetIndex + l - CurrCharacter) % l - (hs2.TargetIndex + l - CurrCharacter) % l);
            return hss.GroupBy(hs => hs.TargetIndex).Select(hsg => new HealSender(TeamIndex, hsg.Sum(hs => hs.Amount), hsg.Key)).ToList();
        }
        /// <summary>
        /// 为了偷懒，使用了DamageVariable作为了治疗<br/>
        /// 仅TargetIndex TargetExcept Damage有效
        /// </summary>
        public void Heal(IDamageSource ds, params DamageVariable[] dvs)
        {
            DamageVariable[] dvs_person = dvs.Select(p => new DamageVariable(ds.DamageSource, p.Element, p.Damage, (p.TargetIndex + CurrCharacter) % Characters.Length, p.TargetExcept)).ToArray();
            List<HealSender> hss = MergeHeal(ds, dvs_person);
            foreach (var hs in hss)
            {
                var c = Characters[hs.TargetIndex];
                hs.Amount = int.Min(c.Card.MaxHP - c.HP, hs.Amount);
                c.HP += hs.Amount;
                Game.BroadCast(ClientUpdateCreate.CharacterUpdate.HealUpdate(TeamIndex, hs.TargetIndex, hs.Amount));
            }
            foreach (var hs in hss)
            {
                Game.EffectTrigger(hs, null);
            }
        }
    }
}
