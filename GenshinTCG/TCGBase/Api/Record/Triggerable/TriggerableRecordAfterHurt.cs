namespace TCGBase
{
    public record TriggerableRecordAfterHurt : TriggerableRecordWithAction
    {
        public TriggerableRecordAfterHurt(List<ActionRecordBase> action) : base(TriggerableType.AfterHurt, action)
        {
        }
        public override AbstractCustomTriggerable GetTriggerable()
        {
            var t = new Triggerable(SenderTag.AfterHurt.ToString());
            t.Action = (me, p, s, v) =>
            {
                if (true)
                {

                }
            };
            return t;
        }
    }
}
