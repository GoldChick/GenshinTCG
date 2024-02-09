namespace TCGBase
{
    /// <summary>
    /// 主动技能
    /// </summary>
    public record TriggerableRecordCustom : TriggerableRecordBase
    {
        public string Name { get; }
        public TriggerableRecordCustom(string name) : base(TriggerableType.Custom)
        {
            Name = name;
        }
        public override AbstractCustomTriggerable GetTriggerable() => Registry.Instance.CustomTriggerable[Name];
    }
}
