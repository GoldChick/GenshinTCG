using System.Diagnostics;

namespace TCGBase
{
    /// <summary>
    /// 不同的参数之间用-分隔
    /// </summary>
    public enum CharacterUseSkillType
    {
        /// <summary>
        /// 附属该状态的角色，若无，则为出战角色
        /// </summary>
        CurrCharacter,
        NotCurrCharacter,
        /// <summary>
        /// 我方所有角色
        /// </summary>
        AllMyCharacter,
        /// <summary>
        /// 对方所有角色
        /// </summary>
        AllEnemyCharacter,
        /// <summary>
        /// 场上所有角色
        /// </summary>
        AllGameCharacter,
        /// <summary>
        /// 指定我方角色; 参数:Character.NameID
        /// </summary>
        CertainCharacter,
        NotCertainCharacter,
    }
    public static class TriggerBasePreset
    {
        public static List<Character> GetCharacter(AbstractTeam team, AbstractPersistent persistent, CharacterUseSkillType type, string certain)
        {
            List<Character> list = new();
            switch (type)
            {
                case CharacterUseSkillType.CurrCharacter:
                    var c = team.Characters.ElementAtOrDefault(persistent.PersistentRegion) ?? team.Characters.ElementAtOrDefault(team.CurrCharacter);
                    if (c != null)
                    {
                        list.Add(c);
                    }
                    break;
                case CharacterUseSkillType.NotCurrCharacter:
                    break;
                case CharacterUseSkillType.AllMyCharacter:
                    list.AddRange(team.Characters);
                    break;
                case CharacterUseSkillType.AllEnemyCharacter:
                    list.AddRange(team.Enemy.Characters);
                    break;
                case CharacterUseSkillType.AllGameCharacter:
                    list.AddRange(team.Characters);
                    list.AddRange(team.Enemy.Characters);
                    break;
                case CharacterUseSkillType.CertainCharacter:
                    break;
                case CharacterUseSkillType.NotCertainCharacter:
                    break;
            }
            return list;
        }
        /// <summary>
        /// costs: eg : "pyro=1,void=2"
        /// </summary>
        public static CostInit GetCost(string costs)
        {
            var cc = new CostCreate();
            foreach (var cost in costs.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var cs = cost.Split("=");
                if (Enum.TryParse(cs[0], out ElementCategory element))
                {
                    cc.Add(element, int.Parse(cs[1]));
                }
            }
            return cc.ToCostInit();
        }
        public static DamageRecord GetDamage(string arg)
        {
            var damages = arg.Split("+");
            DamageRecord dv = new(0, 1);
            DamageRecord curr = dv;
            foreach (var damage in damages)
            {
                curr.SubDamage = GetSingleDamage(damage);
                curr = curr.SubDamage;
            }
            if (dv.SubDamage == null)
            {
                throw new ArgumentException("TriggerBasePreset.GetDamage():传入了null伤害！");
            }
            return dv.SubDamage;
        }
        /// <summary>
        /// arg: 类似"pyro-3(-0-targetonly-enemy)" 班尼特e，2-5项，只以-作为分隔
        /// </summary>
        private static DamageRecord GetSingleDamage(string arg)
        {
            var args = arg.Split("-", StringSplitOptions.RemoveEmptyEntries).Append("0").Append("0").Append("0");
            Debug.Assert(args.Count() >= 5, $"TriggerPreset.DoDamage: Damage必须要至少指定[Element]和[Damage]，并且以-作为分隔！");
            return new(Enum.Parse<DamageElement>(args.ElementAt(0)), int.Parse(args.ElementAt(1)), int.Parse(args.ElementAt(2))
                    , Enum.Parse<DamageTargetArea>(args.ElementAt(3)), Enum.Parse<DamageTargetTeam>(args.ElementAt(4)));
        }
    }
}
