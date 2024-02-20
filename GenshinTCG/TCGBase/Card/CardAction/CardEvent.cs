namespace TCGBase
{
    public class CardEvent : AbstractCardAction, ITargetSelector
    {
        /// <summary>
        /// 将按照顺序依次选取<br/>
        /// 如:[诸武精通]:{Character_Me,Character_Me}<br/>
        /// [送你一程]:{Summon}<br/>
        /// </summary>
        public List<TargetDemand> TargetDemands { get; }
        public CardEvent(CardRecordAction record) : base(record)
        {
            record.Select.ForEach(s =>
            {
                //TODO: card event select
            });
            TargetDemands = new();
        }
    }
}
