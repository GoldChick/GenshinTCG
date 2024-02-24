namespace TCGBase
{
    public record CardRecordEffect : CardRecordBase
    {
        public int InitialUseTimes { get; }
        public int MaxUseTimes { get; }
        public bool CustomDesperated { get; }
        public CardRecordEffect(List<TriggerableRecordBase> skillList, int maxUseTimes, bool customDesperated = false, List<ModifierRecordBase>? modlist = null, int initialUseTimes = -1, List<string>? tags = null, bool hidden = false, CardType cardtype = CardType.Effect) : base(hidden, cardtype, skillList, tags, modlist)
        {
            MaxUseTimes = int.Max(maxUseTimes, 0);
            if (initialUseTimes > MaxUseTimes)
            {
                MaxUseTimes = initialUseTimes;
                InitialUseTimes = initialUseTimes;
            }
            else if (initialUseTimes > 0)
            {
                InitialUseTimes = initialUseTimes;
            }
            else
            {
                InitialUseTimes = MaxUseTimes;
            }
            CustomDesperated = customDesperated;
        }
        public CardEffect GetCard() => new CardEffect(this);
    }
}
