namespace TCGBase
{
    internal sealed class CardEvent : AbstractCardEvent
    {
        public override string Namespace { get; }
        public override List<TargetDemand> TargetDemands { get; }
        public override CostInit Cost { get; }
        public CardEvent(CardRecordEvent record, string @namespace) : base(record)
        {
            record.Select.ForEach(s =>
            {
            });
            TargetDemands = new();
            //record.Select
            Namespace = @namespace;
            CostCreate create = new();
            foreach (var c in record.Cost)
            {
                create.Add(c.Type, c.Count);
            }
            Cost = create.ToCostInit();
        }
    }
}
