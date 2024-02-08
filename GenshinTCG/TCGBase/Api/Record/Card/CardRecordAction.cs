namespace TCGBase
{
    public record CardRecordAction : CardRecordBase
    {
        public int MaxNumPermitted { get; }
        public List<CostRecord> Cost { get; }
        public CardRecordAction(string nameID, CardType cardType, List<string> skillList, List<string> tags, List<CostRecord>? cost = null, bool hidden = false, int maxNumPermitted = 2) : base(nameID, hidden, cardType, skillList, tags)
        {
            MaxNumPermitted = maxNumPermitted;
            Cost = cost ?? new();
        }
        public override AbstractCardBase GetCard() => new CardAction(this);
    }
}
