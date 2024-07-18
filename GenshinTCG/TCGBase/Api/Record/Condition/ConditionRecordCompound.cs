namespace TCGBase
{
    public record class ConditionRecordCompound : ConditionRecordBase
    {
        public List<ConditionRecordBase> Value { get; }
        public ConditionRecordCompound(List<ConditionRecordBase>? value = null, bool not = false, ConditionRecordBase? or = null) : base(ConditionType.Compound, not, or)
        {
            Value = value ?? new();
        }
        protected override bool GetPredicate(PlayerTeam me, Persistent p, SimpleSender s, AbstractVariable? v)
        {
            return Value.All(condition => condition.Valid(me, p, s, v));
        }
    }
}