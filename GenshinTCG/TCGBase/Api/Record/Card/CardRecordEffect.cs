namespace TCGBase
{
    public record CardRecordEffect : CardRecordBase
    {
        public int InitialUseTimes { get; }
        public int MaxUseTimes { get; }
        public CardRecordEffect(List<TriggerableRecordBase> skillList, int maxUseTimes, int initialUseTimes = -1, List<string>? tags = null, bool hidden = false, CardType cardtype = CardType.Effect) : base(hidden, cardtype, skillList, tags)
        {
            MaxUseTimes = int.Max(maxUseTimes, 0);
            InitialUseTimes = initialUseTimes > 0 ? initialUseTimes : MaxUseTimes;
        }
        public AbstractCardEffect GetCard() => new CardEffect(this);
    }
}
