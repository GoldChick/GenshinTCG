namespace TCGBase
{
    public record CardRecordCharacter : CardRecordBase
    {
        public CardRecordCharacter(string nameID, int maxHP, int maxMP, List<TriggerableRecordBase> skillList, List<string> tags, bool hidden = false, CardType cardType = CardType.Character) : base(nameID, hidden, cardType, skillList, tags)
        {
            MaxHP = maxHP;
            MaxMP = maxMP;
        }

        public int MaxHP { get; }
        public int MaxMP { get; }
    }
}
