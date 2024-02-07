namespace TCGBase
{
    public record class SelectRecordCharacter : SelectRecordBase
    {
        public CharacterTargetType CharacterTargetType { get; }
        public SelectRecordCharacter() : base(SelectType.Character)
        {
        }
    }
}
