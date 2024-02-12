namespace TCGBase
{
    public record class ActionRecordSetData : ActionRecordBaseWithTarget
    {
        /// <summary>
        /// 如果不为null，就将persistent.data改为此Data
        /// </summary>
        public List<int>? Data { get; }
        public ActionRecordSetData(List<int>? data = null, TargetRecord? target = null, List<TargetRecord>? when = null) : base(TriggerType.SetData, target, when)
        {
            Data = data;
        }
        public override EventPersistentHandler? GetHandler(AbstractCustomTriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                Target.GetTargets(me, p, s, out var _).ForEach(pe =>
                {
                    if (Data != null)
                    {
                        pe.Data = Data;
                    }
                    else if (pe.Data == null)
                    {
                        pe.Data = new();

                    }
                });
            };
        }
    }
}
