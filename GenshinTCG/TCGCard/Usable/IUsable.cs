using TCGBase;

namespace TCGCard
{
    /// <summary>
    /// 可以使用/打出的东西
    /// </summary>
    public interface IUsable : IDiceCost
    {
        /// <summary>
        /// 是否满足额外的打出条件（不包括骰子条件）
        /// </summary>
        /// <returns></returns>
        public bool CanBeUsed();
        /// <summary>
        /// 打出后发生什么
        /// </summary>
        public void AfterUseAction();
    }
}
