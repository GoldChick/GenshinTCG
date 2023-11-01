namespace TCGBase
{
    /// <summary>
    /// 可以拿在手中被使用的卡牌
    /// </summary>
    public abstract class AbstractCardAction : AbstractCardBase
    {
        /// <summary>
        /// 允许携带的最大数量<br/>
        /// 默认为2
        /// </summary>
        public virtual int MaxNumPermitted { get => 2; }
        public abstract int[] Costs { get; }
        public virtual bool CostSame { get => true; }

        public abstract void AfterUseAction(PlayerTeam me, int[]? targetArgs = null);

        /// <summary>
        /// TODO:还没写选卡
        /// //是否可以加入卡组里
        /// </summary>
        /// <returns></returns>
        public virtual bool CanBeArmed() => true;
        /// <summary>
        /// 是否满足额外的打出条件（不包括骰子条件）
        /// </summary>
        public virtual bool CanBeUsed(PlayerTeam me, int[]? targetArgs = null) => true;
    }
}
