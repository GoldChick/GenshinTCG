namespace TCGBase
{
    public record TriggerableRecordOnCharacterOn : TriggerableRecordBase
    {
        public bool Once { get; }
        public TriggerableRecordOnCharacterOn(List<ActionRecordBase> action, List<ConditionRecordBase>? when = null, bool once = false) : base(TriggerableType.OnCharacterOn, action, when)
        {
            Once = once;
        }
        protected override EventPersistentHandler? Get(AbstractTriggerable triggerable)
        {
            return base.Get(triggerable);
        }
    }
}
