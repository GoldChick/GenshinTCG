namespace TCGBase
{
    public record class ActionRecordCounter : ActionRecordBaseWithTarget
    {
        public int Add { get; }
        public int Set { get; }
        public ActionRecordCounter(int add = 0, int set = -1, TargetRecord? target = null, List<ConditionRecordBase>? when = null) : base(TriggerType.Effect, target, when)
        {
            Add = add;
            Set = set;
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            if (Target.Type is not TargetType.Character)
            {
                var targets = Target.GetTargets(me, p, s, v, out _);
                foreach (var t in targets)
                {
                    if (t is not Character)
                    {
                        Modifier(t);
                    }
                }
            }
            else if (p is not Character)
            {
                Modifier(p);
            }
        }
        private void Modifier(Persistent p)
        {
            if (Set >= 0)
            {
                p.AvailableTimes = Set;
            }
            else
            {
                p.AvailableTimes += Add;
            }
        }
    }
}
