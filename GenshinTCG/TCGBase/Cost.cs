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
    }
}
