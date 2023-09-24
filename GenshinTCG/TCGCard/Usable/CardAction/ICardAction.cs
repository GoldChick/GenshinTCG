using TCGGame;

namespace TCGCard
{
    /// <summary>
    /// 可以拿在手中被使用的卡牌
    /// </summary>
    public interface ICardAction : ICardServer, IUsable<PlayerTeam>
    {
        /// <summary>
        /// 允许携带的最大数量<br/>
        /// @deprecated 锁定在2
        /// </summary>
        public int MaxNumPermitted { get; }

        /// <summary>
        /// TODO:还没写选卡
        /// </summary>
        /// <returns></returns>
        public virtual bool CanBeArmed()//是否可以加入卡组里
                => true;
        /// <summary>
        /// 是否满足额外的打出条件（不包括骰子条件）
        /// </summary>
        public bool CanBeUsed(PlayerTeam me, int[]? targetArgs = null);
    }
}
