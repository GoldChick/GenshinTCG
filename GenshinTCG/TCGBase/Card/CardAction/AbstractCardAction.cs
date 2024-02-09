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
        public virtual bool CanBeArmed(List<AbstractCardCharacter> chars) => true;
        /// <summary>
        /// 用于custom，会自动生成在SenderTagInner.UseCard下<br/>
        /// 如果想加一些其他的触发，可以自行维护TriggerableList
        /// </summary>
        public virtual void AfterUseAction(PlayerTeam me, List<Persistent> targets) { }
        /// <summary>
        /// 是否满足额外的打出条件（不包括骰子条件）
        /// </summary>
        public virtual bool CanBeUsed(PlayerTeam me) => true;
        public abstract CostInit Cost { get; }
        protected private AbstractCardAction() : base("null")
        {
            TriggerableList.Add(SenderTagInner.UseCard, (me, p, s, v) => AfterUseAction(me, new() { p }));
        }
        protected private AbstractCardAction(CardRecordAction record) : base(record)
        {
            MaxNumPermitted = record.MaxNumPermitted;
            FastAction = !record.Tags.Contains(CardTag.Slowly.ToString());
        }
    }
}
