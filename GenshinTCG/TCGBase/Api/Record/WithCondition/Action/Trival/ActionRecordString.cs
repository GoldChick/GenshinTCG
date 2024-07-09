namespace TCGBase
{
    public enum DataSetType
    {
        Add,
        Clear,
    }
    public record class ActionRecordString : ActionRecordBase
    {
        public string Value { get; }
        public ActionRecordString(TriggerType type, string? value = null, List<ConditionRecordBase>? when = null) : base(type, when)
        {
            Value = value ?? "add";
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            switch (Type)
            {
                case TriggerType.Element:
                    if (Enum.TryParse(Value, out DamageElement ele))
                    {
                        me.AttachElement(p, triggerable, ele, new List<int>() { p.PersistentRegion }, false);
                    }
                    break;
            }
        }
    }
}
