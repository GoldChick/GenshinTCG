namespace TCGBase
{
    internal sealed class CardSupport : AbstractCardSupport
    {
        public override CostInit Cost { get; }
        public CardSupport(CardRecordAction record) : base(record)
        {
            Cost = new CostCreate(record.Cost).ToCostInit();
        }
    }
}
