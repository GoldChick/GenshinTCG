namespace TCGBase
{
    public record class ConditionRecordTarget : ConditionRecordBase
    {
        public TargetRecord Target { get; }
        public int Value { get; }
        public ConditionRecordTarget(ConditionType type, TargetRecord target, int value = 0, bool not = false, ConditionRecordBase? or = null) : base(type, not, or)
        {
            Target = target;
            Value = value;
        }
        protected override bool GetPredicate(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            return Type switch
            {
                ConditionType.AnyTarget => Target.GetTargets(me, p, s, v, out _).Any(),
                ConditionType.AnyTargetWithSameIndex => Target.GetTargets(me, p, s, v, out _) is List<Persistent> ps && ps.Any() && ps.TrueForAll(tar => tar.PersistentRegion == p.PersistentRegion),
                ConditionType.TargetCount => Target.GetTargets(me, p, s, v, out _).Count == Value,
                ConditionType.CanBeAppliedFrom => me.Characters.ElementAtOrDefault(p.PersistentRegion) is Character c && Target.GetTargets(me, p, s, v, out var _).Any()
                    && Target.GetTargets(me, p, s, v, out var team).All(per => per.CardBase is ITargetSelector its && team.GetTargetValid(its.TargetDemands).Any(list => list.Contains(c))),

                _ => base.GetPredicate(me, p, s, v)
            };
        }
    }
}
