﻿namespace TCGBase
{
    /// <summary>
    /// 七圣有三大卡牌....
    /// </summary>
    public enum CardActionCategory
    {
        Event,
        Support,
        Equipment,
    }
    /// <summary>
    /// 可以拿在手中被使用的卡牌
    /// </summary>
    public abstract class AbstractCardAction : AbstractCardPersistent
    {
        /// <summary>
        /// 允许携带的最大数量<br/>
        /// 默认为2
        /// </summary>
        public virtual int MaxNumPermitted { get => 2; }
        /// <summary>
        /// 是否快速行动，默认为true
        /// </summary>
        public virtual bool FastAction { get => true; }
        public abstract CostInit Cost { get; }
        /// <summary>
        /// //是否可以加入卡组里
        /// </summary>
        public virtual bool CanBeArmed(List<AbstractCardCharacter> chars) => true;
        public abstract void AfterUseAction(PlayerTeam me, int[] targetArgs);
        /// <summary>
        /// 是否满足额外的打出条件（不包括骰子条件）<br/>
        /// 如果实现ITargetSelector，且为单目标，可以借助这个方法给virtual类的target用
        /// </summary>
        public virtual bool CanBeUsed(PlayerTeam me, int[] targetArgs) => true;
        private protected AbstractCardAction():base(null)
        {
            //TODO: record
        }
    }
}
