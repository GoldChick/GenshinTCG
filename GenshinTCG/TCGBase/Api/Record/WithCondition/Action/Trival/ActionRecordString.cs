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
                    me.Game.EffectTrigger(new SimpleSourceSender(me.TeamID, Value, p));
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
                    else if (Value.StartsWith("add", true, null))
                    {
                        if (int.TryParse(Value[3..], out int num))
                        {
                            p.Data.Add(num);
                        }
                    }
                    break;
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
