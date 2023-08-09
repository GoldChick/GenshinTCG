using TCGBase;

namespace TCGCard
{
    /// <summary>
    /// 可以拿在手中被使用的卡牌
    /// </summary>
    public interface ICardOwnalbe : ICardServer, IUsable
    {
        /// <summary>
        /// 允许携带的最大数量<br/>
        /// @deprecated 锁定在2
        /// </summary>
        public int MaxNumPermitted { get; }

        public bool CanBeArmed();//是否可以加入卡组里
    }
}
