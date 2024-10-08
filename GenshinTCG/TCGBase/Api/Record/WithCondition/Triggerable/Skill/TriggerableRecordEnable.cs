﻿namespace TCGBase
{
    /// <summary>
    /// 如果EeedEnable为true，则第一次不会触发<br/>
    /// 说的就是你！旋火轮！<br/>
    /// 会占用persistent的Data，往里面加一个0
    /// </summary>
    public record TriggerableRecordEnable : TriggerableRecordBase
    {
        public bool NeedEnable { get; }
        public TriggerableRecordEnable(TriggerableType type, List<ActionRecordBase> action, bool needEnable = false, List<ConditionRecordBase>? when = null) : base(type, action, when)
        {
            NeedEnable = needEnable;
        }
        protected override void Get(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, SimpleSender s, AbstractVariable? v)
        {
            if (NeedEnable && p.Data.Count == 0)
            {
                p.Data.Add(0);
            }
            else
            {
                base.Get(triggerable, me, p, s, v);
            }
        }
    }
}
