using System.Text.Json;
using System.Text.Json.Serialization;

namespace TCGBase
{
    public class CostVariable : AbstractVariable
    {
        [JsonIgnore]
        internal List<SingleCostVariable> Costs { get; }
        public int[] DiceCost { get; }
        [JsonIgnore]
        public int CostSum => Costs.Sum(scv => scv.Count);
        public CostVariable()
        {
            Costs = new();
            DiceCost = new int[11];
        }
        public CostVariable(CostInit cost) : this(cost.DiceCost)
        {
        }
        [JsonConstructor]
        public CostVariable(int[] dicecost)
        {
            Costs = new();
            DiceCost = new int[11];
            for (int i = 0; i < dicecost.Length; i++)
            {
                if (dicecost[i] > 0)
                {
                    Costs.Add(new SingleCostVariable((ElementCategory)i, dicecost[i]));
                    DiceCost[i] = dicecost[i];
                }
            }
        }
        internal void Mod(PlayerTeam me, DiceModifierSender sender)
        {
            foreach (var singleCostVariable in Costs)
            {
                if (singleCostVariable.Type == ElementCategory.Void)
                {
                    me.InstantTrigger(DiceModifierType.Void.ToString(), sender, singleCostVariable);
                }
                me.InstantTrigger(DiceModifierType.Color.ToString(), sender, singleCostVariable);
                me.InstantTrigger(DiceModifierType.Any.ToString(), sender, singleCostVariable);
                me.InstantTrigger(DiceModifierType.Late.ToString(), sender, singleCostVariable);

                DiceCost[(int)singleCostVariable.Type] = singleCostVariable.Count;
            }
        }
        /// <summary>
        /// 判断提供的dices是否能<b>恰好</b>满足需要
        /// </summary>
        /// <param name="supply">选中的元素骰，万能 冰水火雷岩草风</param>
        public bool DiceEqualTo(int[]? supply)
        {
            supply ??= new int[9];
            if (supply.All(p => p >= 0) && supply.Sum() == CostSum)
            {
                if (Costs.ElementAtOrDefault(0)?.Type == ElementCategory.Trivial)
                {
                    //同色
                    int num = supply.Where(i => i > 0).Count();
                    return num <= 1 || (num == 2 && supply.ElementAtOrDefault(0) > 0);
                }
                else
                {
                    //杂色+某(几)种指定颜色
                    //只需要满足 总数量相同+万能能够满足缺少的元素
                    return supply[0] >= Costs.Where(scv => (int)scv.Type <= 7).Select(scv => scv.Count -= int.Min(scv.Count, supply.ElementAtOrDefault((int)scv.Type))).Sum();
                }
            }
            return false;
        }
    }
    /// <summary>
    /// 为卡牌、技能构建CostInit的中间态，经过处理后得到最终结果<br/><br/>
    /// 特殊规则<b>需要[同色]时，不能再需要[杂色]或[元素骰]</b>
    /// </summary>
    public class CostCreate
    {
        protected int[] _costs;
        /// <summary>
        /// 同色 冰水火雷岩草风 杂色 充能 秘传点
        /// </summary>
        public int[] DiceCost { get => _costs; }
        public int MPCost => _costs[9];
        public int Legend => _costs[10];
        public CostCreate()
        {
            _costs = new int[11];
        }
        internal CostCreate(List<SingleCostVariable>? costs) : this()
        {
            costs?.ForEach(c => Add(c.Type, c.Count));
        }
        public CostInit ToCostInit()
        {
            if (_costs[0] > 0)
            {
                for (int i = 1; i < 9; i++)
                {
                    _costs[i] = 0;
                }
            }
            return new(_costs);
        }
        public CostCreate Add(ElementCategory element, int num)
        {
            _costs[(int)element] += num;
            return this;
        }
    }
    /// <summary>
    /// 创建[费用]，<see cref="CostCreate"/><br/><br/>
    /// 特殊规则<b>需要[同色]时，不能再需要[杂色]或[元素骰]</b>
    /// </summary>
    public class CostInit : CostCreate
    {
        public CostInit() : base()
        {
        }
        /// <summary>
        ///  0 同色(有同色则其他骰都为0)<br/>
        ///  1-7 冰水火雷岩草风<br/>
        ///  8 杂色<br/>
        ///  9 充能<br/>
        ///  10 秘传
        /// </summary>
        internal CostInit(int[] dicecost) : base()
        {
            for (int i = 0; i < 11 && i < dicecost.Length; i++)
            {
                _costs[i] = dicecost[i];
            }
        }
        /// <summary>
        ///  0 同色(有同色则其他骰都为0)<br/>
        ///  1-7 冰水火雷岩草风<br/>
        ///  8 杂色<br/>
        ///  9 充能<br/>
        ///  10 秘传
        /// </summary>
        public int[] GetCost() => DiceCost.ToArray();
        public CostVariable ToCostVariable() => new(this);
    }
}
