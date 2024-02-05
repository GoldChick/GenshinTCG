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
        public virtual TargetDemand[] TargetDemands => Array.Empty<TargetDemand>();
        protected AbstractCardEvent()
        {
        }
        protected private AbstractCardEvent(CardRecordEvent record) : base(record)
        {

        }
    }
}
