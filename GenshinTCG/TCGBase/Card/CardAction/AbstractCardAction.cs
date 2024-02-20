namespace TCGBase
{
    /// <summary>
    /// 可以拿在手中被使用的卡牌
    /// </summary>
    public abstract class AbstractCardAction : AbstractCardBase, ICostable
    {
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
        public virtual bool CanBeArmed(List<CardCharacter> chars) => true;
        /// <summary>
        /// 是否满足额外的打出条件（不包括骰子条件）
        /// </summary>
        public virtual bool CanBeUsed(PlayerTeam me) => true;
        public CostInit Cost { get; }
        protected private AbstractCardAction(CardRecordAction record) : base(record)
        {
            MaxNumPermitted = record.MaxNumPermitted;
            FastAction = !record.Tags.Contains(CardTag.Slowly.ToString());
            Cost = new CostCreate(record.Cost).ToCostInit();
        }
    }
}
