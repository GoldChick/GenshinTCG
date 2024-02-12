namespace TCGBase
{
    public record class ConditionRecordString : ConditionRecordBase
    {
        public string Value { get; }
        public ConditionRecordString(ConditionType type, string? value = null, bool not = false) : base(type, not)
        {
            Value = value ?? "test";
        }
        public override Func<PlayerTeam, AbstractSender, AbstractVariable, bool> GetPredicate()
        {
            return Type switch
            {
                ConditionType.Element => (me, s, v) => v is DamageVariable dv && dv.Element.ToString() == Value,
                ConditionType.Reaction => (me, s, v) => v is DamageVariable dv && dv.Reaction.ToString() == Value,
                ConditionType.SkillType => (me, s, v) => s is PreHurtSender phs && phs.RootSource is ISkillable skill && skill.SkillCategory.ToString() == Value,
                ConditionType.Related => (me, s, v) => v is DamageVariable dv && (((DamageElement)((int)dv.Reaction / 10)).ToString() == Value || ((DamageElement)((int)dv.Reaction % 10)).ToString() == Value),
                _ => base.GetPredicate()
            };
        }
        public override Func<PlayerTeam, Persistent, bool> GetPersistentPredicate()
        {
            return Type switch
            {
                ConditionType.HasEffect => (me, p) => p is Character c && c.Effects.Contains(Value),
                ConditionType.HasEffectWithTag => (me, p) => p is Character c && c.Effects.Contains(ef => ef.CardBase.Tags.Contains(Value)),
                ConditionType.HasTag => (me, p) => p.CardBase.Tags.Contains(Value),
                _ => base.GetPersistentPredicate()
            };
        }
    }
}