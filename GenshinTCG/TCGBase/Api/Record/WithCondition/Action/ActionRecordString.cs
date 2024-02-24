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
                case TriggerType.Trigger:
                    me.Game.EffectTrigger(new SimpleSender(me.TeamIndex, Value));
                    break;
                case TriggerType.SetData:
                    if (Enum.TryParse(Value, true, out DataSetType type))
                    {
                        switch (type)
                        {
                            case DataSetType.Add:
                                p.Data.Add(0);
                                break;
                            case DataSetType.Clear:
                                p.Data.Clear();
                                break;
                        }
                    }
                    break;
            }
        }
    }
    public record class ActionRecordTrigger : ActionRecordString
    {
        public ActionRecordTrigger(string? value = null, List<ConditionRecordBase>? when = null) : base(TriggerType.Trigger, value, when)
        {
        }
    }
}
