using System.Text.Json.Serialization;

namespace TCGBase
{
    public class CostVariable : AbstractVariable
    {
        protected int[] _costs;
        /// <summary>
        /// 同色 冰水火雷岩草风 杂色<br/>
        /// 请不要通过此属性修改
        /// </summary>
        public int[] DiceCost { get => _costs; }
        public int MPCost { get; protected set; }
        public CostVariable()
        {
            _costs = new int[9];
        }
        public CostVariable(CostInit init) : this(init.DiceCost, init.MPCost)
        {

        }
        [JsonConstructor]
        public CostVariable(int[] dicecost, int mpcost) : this()
        {
            for (int i = 0; i < 9 && i < dicecost.Length; i++)
            {
                _costs[i] = dicecost[i];
            }
            MPCost = mpcost;
        }
        /// <summary>
        /// 判断提供的dices是否能<b>恰好</b>满足需要
        /// </summary>
        /// <param name="supply">选中的元素骰，万能 冰水火雷岩草风</param>
        public bool DiceEqualTo(int[]? supply)
        {
            supply ??= new int[9];
            if (supply.All(p => p >= 0) && supply.Sum() == _costs.Sum())
            {
                if (_costs[0] > 0)
                {
                    //同色
                    int num = supply.Where(i => i > 0).Count();
                    return num <= 1 || (num == 2 && supply.ElementAtOrDefault(0) > 0);
                }
                else
                {
                    //杂色+某(几)种指定颜色
                    //只需要满足 总数量相同+万能能够满足缺少的元素
                    return supply[0] >= _costs.Select((c, index) => c -= int.Min(c, supply.ElementAtOrDefault(index))).ToArray()[1..8].Sum();
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
        /// 同色 冰水火雷岩草风 杂色
        /// </summary>
        public int[] DiceCost { get => _costs; }
        /// <summary>
        /// 对于[技能]，直接填写数值即可<br/>
        /// 对于[卡牌]，需要实现<see cref="IEnergyConsumerCard"/>，指定额外Target的中角色的index（天赋卡默认index=0），否则默认为出战角色
        /// </summary>
        public int MPCost { get; protected set; }
        public CostCreate()
        {
            _costs = new int[9];
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
            return new(_costs, MPCost);
        }
        public CostCreate Void(int num)
        {
            _costs[8] += num;
            return this;
        }
        public CostCreate Same(int num)
        {
            _costs[0] += num;
            return this;
        }
        public CostCreate Cryo(int num)
        {
            _costs[1] += num;
            return this;
        }
        public CostCreate Hydro(int num)
        {
            _costs[2] += num;
            return this;
        }
        public CostCreate Pyro(int num)
        {
            _costs[3] += num;
            return this;
        }
        public CostCreate Electro(int num)
        {
            _costs[4] += num;
            return this;
        }
        public CostCreate Geo(int num)
        {
            _costs[5] += num;
            return this;
        }
        public CostCreate Dendro(int num)
        {
            _costs[6] += num;
            return this;
        }
        public CostCreate Anemo(int num)
        {
            _costs[7] += num;
            return this;
        }
        public CostCreate MP(int num)
        {
            MPCost += num;
            return this;
        }
    }
    /// <summary>
    /// 创建[费用]，<see cref="CostCreate"/><br/><br/>
    /// 特殊规则<b>需要[同色]时，不能再需要[杂色]或[元素骰]</b>
    /// </summary>
    public class CostInit : CostCreate
    {
        public CostInit():base()
        {
        }
        internal CostInit(int[] dicecost, int mpcost) : base()
        {
            for (int i = 0; i < 9 && i < dicecost.Length; i++)
            {
                _costs[i] = dicecost[i];
            }
            MPCost = mpcost;
        }
        public CostVariable ToCostVariable() => new(this);
    }
}
