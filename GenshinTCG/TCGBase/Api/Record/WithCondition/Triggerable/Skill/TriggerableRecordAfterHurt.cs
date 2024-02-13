﻿namespace TCGBase
{
    public record TriggerableRecordAfterHurt : TriggerableRecordBase
    {
        public TriggerableRecordAfterHurt(List<ActionRecordBase> action) : base(TriggerableType.AfterHurt, action)
        {
        }
        public override AbstractTriggerable GetTriggerable()
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