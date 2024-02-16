namespace TCGBase
{
    public record class ConditionRecordAnyTarget : ConditionRecordBase
    {
        public TargetRecord Target { get; }
        public ConditionRecordAnyTarget(TargetRecord target, bool not = false, ConditionRecordBase? or = null) : base(ConditionType.AnyTarget, not, or)
        {
            Target = target;
        }
        protected override bool GetPredicate(PlayerTeam me, Persistent? p, AbstractSender? s, AbstractVariable? v)
        {
            return Target.GetTargets(me, p, s, v, out _).Any();
        }
    }
}
