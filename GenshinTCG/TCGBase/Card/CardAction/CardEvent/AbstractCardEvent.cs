namespace TCGBase
{
    /// <summary>
    /// default add effect to team
    /// </summary>
    public abstract class AbstractCardEvent : AbstractCardAction, ITargetSelector
    {
        /// <summary>
        /// 将按照顺序依次选取<br/>
        /// 如:[诸武精通]:{Character_Me,Character_Me}<br/>
        /// [送你一程]:{Summon}<br/>
        /// </summary>
        public virtual List<TargetDemand> TargetDemands => new();
        protected AbstractCardEvent()
        {
        }
        protected private AbstractCardEvent(CardRecordAction record) : base(record)
        {

        }
    }
}
