using TCGBase;
using TCGGame;

namespace TCGCard
{
    /// <summary>
    /// 可以拿在手中被使用的卡牌
    /// </summary>
    public interface ICardAction : ICardServer, IUsable
    {
        /// <summary>
        /// 允许携带的最大数量<br/>
        /// @deprecated 锁定在2
        /// </summary>
        public int MaxNumPermitted { get; }

        public virtual bool CanBeArmed()//是否可以加入卡组里
        {
            return true;
        }
        /// <summary>
        /// 是否满足额外的打出条件（不包括骰子条件）
        /// </summary>
        /// <returns></returns>
        public bool CanBeUsed(AbstractGame game, int meIndex);
    }
}
