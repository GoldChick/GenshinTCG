namespace TCGBase
{
    public record TriggerableRecordCustom : TriggerableRecordBase
    {
        public string Name { get; }
        public TriggerableRecordCustom(string name) : base(TriggerableType.Custom, null)
        {
            Name = name;
        }
        public override AbstractTriggerable GetTriggerable() => Registry.Instance.CustomTriggerable[Name];
    }
}
