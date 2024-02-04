namespace TCGBase
{
    /// <summary>
    /// 可以拿在手中被使用的卡牌
    /// </summary>
    public abstract class AbstractCardAction : AbstractCardBase
    {
        /// <summary>
        /// 将按照顺序依次选取<br/>
        /// 如:[诸武精通]:{Character_Me,Character_Me}<br/>
        /// [送你一程]:{Summon}<br/>
        /// </summary>
        public virtual TargetDemand[] TargetDemands { get; }
        /// <summary>
        /// 允许携带的最大数量<br/>
        /// </summary>
        public int MaxNumPermitted { get; }
        /// <summary>
        /// 是否快速行动，默认为true
        /// </summary>
        public bool FastAction { get; }
        /// <summary>
        /// //是否可以加入卡组里
        /// </summary>
        public virtual bool CanBeArmed(List<AbstractCardCharacter> chars) => true;
        public abstract void AfterUseAction(PlayerTeam me, int[] targetArgs);
        /// <summary>
        /// 是否满足额外的打出条件（不包括骰子条件）<br/>
        /// 如果实现ITargetSelector，且为单目标，可以借助这个方法给virtual类的target用
        /// </summary>
        public virtual bool CanBeUsed(PlayerTeam me, int[] targetArgs) => targetArgs.Length == TargetDemands.Length;
        protected AbstractCardAction() : base("null")
        {
            //TODO: override
        }
        internal AbstractCardAction(CardRecordAction record) : base(record)
        {
            MaxNumPermitted = record.MaxNumPermitted;
            FastAction = !record.Tags.Contains(CardTag.Slowly.ToString());
            TargetDemands = Array.Empty<TargetDemand>();
        }
    }
}
