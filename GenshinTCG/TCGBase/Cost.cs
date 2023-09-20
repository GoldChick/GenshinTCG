using TCGUtil;

namespace TCGBase
{
    /// <summary>
    /// 支持"X黑"、"X白"、"{x1,x2...}{风，岩...}+X黑"
    /// <br/>
    /// 不支持"X黑+X白"
    /// </summary>
    public interface ICost
    {
        /// <summary>
        /// 万能 冰水火雷岩草风
        /// </summary>
        public int[] Costs { get; }
        /// <summary>
        /// 为True时，仅考虑Cost[0]，即消耗Cost[0]个同色骰
        /// </summary>
        public bool CostSame { get; }
    }
    public class Cost : ICost
    {
        private int[] _costs;
        public bool CostSame { get; init; }
        public int[] Costs { get => _costs; }
        public Cost(bool costSame, params int[] costs)
        {
            CostSame = costSame;
            Normalize.CostNormalize(costs, out _costs);
        }
        /// <summary>
        /// 判断提供的dices是否能恰好满足需要
        /// </summary>
        /// <param name="supply">选中的元素骰，万能 冰水火雷岩草风</param>
        public bool EqualTo(int[]? supply)
        {
            supply ??= new int[8];
            if (supply.All(p => p >= 0) && supply.Sum() == _costs.Sum())
            {
                if (CostSame)
                {
                    //同色=万能+某种颜色
                    int num = supply.Where(i => i > 0).Count();
                    return num <= 1 || (num == 2 && supply[0] > 0);
                }
                else
                {
                    //杂色+某(几)种指定颜色
                    Normalize.CostNormalize(supply, out supply);
                    return supply[0] >= _costs.Select((c, index) => c -= int.Min(c, supply[index])).ToArray()[1..].Sum();
                }
            }
            return false;
        }
    }
}
