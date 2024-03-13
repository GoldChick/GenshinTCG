namespace TCGBase
{
    public record class ActionRecordAorB : ActionRecordBase
    {
        public ActionRecordBase A { get; }
        public ActionRecordBase B { get; }
        public ActionRecordAorB(ActionRecordBase a, ActionRecordBase b,  List<ConditionRecordBase>? when = null) : base(TriggerType.AorB, when)
        {
            A = a;
            B = b;
        }
        public override EventPersistentHandler? GetHandler(AbstractTriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                if ((this as IWhenThenAction).IsConditionValid(me, p, s, v))
                {
                    A.GetHandler(triggerable)?.Invoke(me,p,s,v);
                }
                else
                {
                    B.GetHandler(triggerable)?.Invoke(me, p, s, v);
                }
            };
        }
    }
}
