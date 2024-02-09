namespace TCGBase
{
    internal sealed class CardSupport : AbstractCardSupport
    {
        public override string Namespace { get; }
        public override CostInit Cost { get; }
        public CardSupport(CardRecordSupport record, string @namespace) : base(record)
        {
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
