namespace TCGBase
{
    public record CardRecordAction : CardRecordBase
    {
        public List<TargetRecord> Select { get; }
        public int MaxNumPermitted { get; }
        public List<SingleCostVariable> Cost { get; }
        public CardRecordAction(CardType cardType, List<TriggerableRecordBase> skillList, List<string> tags, List<TargetRecord>? select = null, List<SingleCostVariable>? cost = null, bool hidden = false, int maxNumPermitted = 2, List<ModifierRecordBase>? modlist = null) : base(hidden, cardType, skillList, tags, modlist)
        {
            MaxNumPermitted = maxNumPermitted;
            Cost = cost ?? new();
            Select = select ?? new();
        }
        public virtual AbstractCardAction GetCard() => CardType switch
        {
            CardType.Event => new CardEvent(this),
            CardType.Equipment => new CardEquipment(this),
            CardType.Support => new CardSupport(this),
            _ => throw new NotImplementedException($"UnKnown Action CardType: {CardType}"),
        };
    }
}
