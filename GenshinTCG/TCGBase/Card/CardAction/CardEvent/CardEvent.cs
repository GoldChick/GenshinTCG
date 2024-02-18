namespace TCGBase
{
    internal sealed class CardEvent : AbstractCardEvent
    {
        public override List<TargetDemand> TargetDemands { get; }
        public override CostInit Cost { get; }
        public CardEvent(CardRecordAction record) : base(record)
        {
            record.Select.ForEach(s =>
            {
            });
            TargetDemands = new();
            //record.Select
            Cost = new CostCreate(record.Cost).ToCostInit();
        }
    }
}
