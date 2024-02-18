namespace TCGBase
{
    public record class ActionRecordString : ActionRecordBase
    {
        public string Value { get; }
        public ActionRecordString(TriggerType type, string? value = null, List<ConditionRecordBase>? when = null) : base(type, when)
        {
            Value = value ?? "test";
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            switch (Type)
            {
                case TriggerType.Trigger:
                    me.Game.EffectTrigger(new SimpleSender(me.TeamIndex, Value));
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
