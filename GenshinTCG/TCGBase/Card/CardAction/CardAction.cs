namespace TCGBase
{
    internal sealed class CardAction : AbstractCardAction
    {
        public override CostInit Cost { get; }
        public CardAction(CardRecordAction record) : base(record)
        {
            CostCreate create = new();
            foreach (var c in record.Cost)
            {
                create.Add(c.Type, c.Count);
            }
            Cost = create.ToCostInit();
        }
    }
}
