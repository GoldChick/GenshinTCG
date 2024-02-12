
namespace TCGBase
{
    public record class ConditionRecordBiInt : ConditionRecordBase
    {
        public int Index { get; }
        public int Value { get; }
        public ConditionRecordBiInt(ConditionType type, int value, int index = 0, bool not = false) : base(type, not)
        {
            Index = index;
            Value = value;
        }
        public override Func<PlayerTeam, AbstractSender, AbstractVariable, bool> GetPredicate()
        {
            return Type switch
            {
                ConditionType.DamageMoreThan => (me, s, v) => v is DamageVariable dv && dv.Damage > Value,
                ConditionType.DamageEquals => (me, s, v) => v is DamageVariable dv && dv.Damage == Value,
                _ => base.GetPredicate()
            };
        }
        public override Func<PlayerTeam, Persistent, bool> GetPersistentPredicate()
        {
            return Type switch
            {
                ConditionType.HurtMoreThan => (me, p) => p is Character c && c.CharacterCard.MaxHP - c.HP > Value,
                ConditionType.HurtEquals => (me, p) => p is Character c && c.CharacterCard.MaxHP - c.HP == Value,
                //↓ BiInt
                ConditionType.CounterMoreThan => (me, p) => (p is Character c ? c.SkillCounter.ElementAtOrDefault(Index) : p.AvailableTimes) > Value,
                ConditionType.CounterEquals => (me, p) => (p is Character c ? c.SkillCounter.ElementAtOrDefault(Index) : p.AvailableTimes) == Value,
                _ => base.GetPersistentPredicate()
            };
        }
    }
}