namespace TCGBase
{
    /// <summary>
    /// 可以拿在手中被使用的卡牌
    /// </summary>
    public abstract class AbstractCardAction : AbstractCardBase
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
        public virtual void AfterUseAction(AbstractTeam me, int[] targetArgs) { }
        /// <summary>
        /// 是否满足额外的打出条件（不包括骰子条件）<br/>
        /// 如果实现ITargetSelector，且为单目标，可以借助这个方法给virtual类的target用
        /// </summary>
        public virtual bool CanBeUsed(AbstractTeam me, int[] targetArgs) => true;
        protected private AbstractCardAction() : base("null")
        {
            TriggerableList.Add(SenderTagInner.UseCard, (me, p, s, v) => AfterUseAction(me, new int[] { p.PersistentRegion }));
        }
        protected private AbstractCardAction(CardRecordAction record) : base(record)
        {
            MaxNumPermitted = record.MaxNumPermitted;
            FastAction = !record.Tags.Contains(CardTag.Slowly.ToString());
        }
    }
}
