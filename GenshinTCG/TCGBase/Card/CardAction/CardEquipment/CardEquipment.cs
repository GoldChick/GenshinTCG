namespace TCGBase
{
    internal sealed class CardEquipment : AbstractCardEquipment
    {
        public override CostInit Cost { get; }
        public CardEquipment(CardRecordEquipment record) : base(record)
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
