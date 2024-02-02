using System.Diagnostics;

namespace TCGBase
{
    public static class TriggerBasePreset
    {
        /// <summary>
        /// costs: eg : "pyro=1,void=2"
        /// </summary>
        /// <param name="costs"></param>
        /// <returns></returns>
        public static CostInit GetCost(string costs)
        {
            var cc = new CostCreate();
            foreach (var cost in costs.Split(','))
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
