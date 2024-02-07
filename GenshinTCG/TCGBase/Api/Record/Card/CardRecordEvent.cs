namespace TCGBase
{
    public record CardRecordEvent : CardRecordAction
    {
        public List<SelectRecordBase> Select { get; }
        public CardRecordEvent(string nameID, CardType cardType, List<string> skillList, List<string> tags, List<SelectRecordBase>? select = null, List<CostRecord>? cost = null, bool hidden = false, int maxNumPermitted = 2) : base(nameID, cardType, skillList, tags, cost, hidden, maxNumPermitted)
        {
            Select = select ?? new();
        }
        public override AbstractCardBase GetCard() => new CardEvent(this);
    }
}
