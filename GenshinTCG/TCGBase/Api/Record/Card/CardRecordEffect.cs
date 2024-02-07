namespace TCGBase
{
    public record CardRecordEffect : CardRecordBase
    {
        public int InitialUseTimes { get; }
        public int MaxUseTimes { get; }
        public CardRecordEffect(string nameID, List<string> skillList, int maxUseTimes, int initialUseTimes = -1, List<string>? tags = null, bool hidden = false) : base(nameID, hidden, CardType.Effect, skillList, tags)
        {
            MaxUseTimes = int.Max(maxUseTimes, 0);
            InitialUseTimes = initialUseTimes > 0 ? initialUseTimes : MaxUseTimes;
        }
        public override AbstractCardBase GetCard() => new CardEffect(this);
    }
}
