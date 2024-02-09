namespace TCGBase
{
    internal sealed class CardSupport : AbstractCardSupport
    {
        public override CostInit Cost { get; }
        public CardSupport(CardRecordSupport record) : base(record)
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
