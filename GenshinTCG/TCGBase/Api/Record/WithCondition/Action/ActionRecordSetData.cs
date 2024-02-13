namespace TCGBase
{
    public record class ActionRecordSetData : ActionRecordBaseWithTarget
    {
        /// <summary>
        /// 如果不为null，就将persistent.data改为此Data
        /// </summary>
        public List<int>? Data { get; }
        public ActionRecordSetData(List<int>? data = null, TargetRecord? target = null, List<List<ConditionRecordBase>>? whenany = null) : base(TriggerType.SetData, target, whenany)
        {
            Data = data;
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            Target.GetTargets(me, p, s, v, out var _).ForEach(pe =>
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
        }
    }
}
