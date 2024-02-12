
namespace TCGBase
{
    public record class ConditionRecordTriInt : ConditionRecordBase
    {
        public int Sign { get; }
        public int Index { get; }
        public int Value { get; }
        public ConditionRecordTriInt(ConditionType type, int value, int index = 0, int sign = 0, bool not = false) : base(type, not)
        {
            Index = index;
            Value = value;
            Sign = Math.Sign(sign);
        }
        protected override bool GetPredicate(PlayerTeam me, Persistent? p, AbstractSender? s, AbstractVariable? v)
        {
            return Type switch
            {
                ConditionType.Damage => v is DamageVariable dv && Math.Sign(dv.Damage - Value) == Sign,

                ConditionType.HPLost => p is Character c && Math.Sign(c.CharacterCard.MaxHP - c.HP - Value) == Sign,
                ConditionType.Counter => p != null && Math.Sign((p is Character c ? c.SkillCounter.ElementAtOrDefault(Index) : p.AvailableTimes) - Value) == Sign,
                _ => base.GetPredicate(me, p, s, v)
            };
        }
    }
}