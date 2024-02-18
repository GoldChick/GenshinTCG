namespace TCGBase
{
    public record class ActionRecordSetData : ActionRecordBaseWithTarget
    {
        /// <summary>
        /// 如果不为null，就将persistent.data改为此Data
        /// </summary>
        public List<int>? Data { get; }
        //以下三位均大于等于零有效
        public int Add { get; }
        public int Remove { get; }
        public int RemoveAt { get; }
        public ActionRecordSetData(List<int>? data = null, TargetRecord? target = null, List<ConditionRecordBase>? when = null) : base(TriggerType.SetData, target, when)
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
