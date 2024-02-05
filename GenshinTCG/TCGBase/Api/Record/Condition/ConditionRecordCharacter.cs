namespace TCGBase
{
    public record class ConditionRecordCharacter : ConditionRecordBase
    {
        public CharacterTargetType CharacterTargetType { get; }
        public ConditionRecordCharacter() : base(ConditionType.Character)
        {
        }
    }
}
