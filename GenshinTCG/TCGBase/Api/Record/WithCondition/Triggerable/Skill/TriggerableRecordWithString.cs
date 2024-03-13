namespace TCGBase
{
    public record TriggerableRecordWithString : TriggerableRecordBase
    {
        public string Value { get; }
        public TriggerableRecordWithString(TriggerableType type, string value, List<ActionRecordBase> action, List<ConditionRecordBase>? when = null) : base(type, action, when)
        {
            Value = value;
        }
        public override AbstractTriggerable GetTriggerable()
        {
            return Type switch
            {
                TriggerableType.CustomSenderName => new Triggerable(Value, GetHandler),
                _ => base.GetTriggerable(),
            };
        }
    }
}
