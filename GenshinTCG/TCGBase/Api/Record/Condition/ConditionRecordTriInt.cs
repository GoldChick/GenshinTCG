
namespace TCGBase
{
    public record class ConditionRecordTriInt : ConditionRecordBase
    {
        public int Sign { get; }
        public int Index { get; }
        public int Value { get; }
        public ConditionRecordTriInt(ConditionType type, int value, int index = 0, int sign = 0, bool not = false, ConditionRecordBase? or = null) : base(type, not, or)
        {
            Index = index;
            Value = value;
            Sign = Math.Sign(sign);
        }
        protected override bool GetPredicate(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            return Type switch
            {
                ConditionType.Damage => v is DamageVariable dv && Math.Sign(dv.Amount - Value) == Sign,

                ConditionType.HP => p is Character c && Math.Sign(c.HP - Value) == Sign,
                ConditionType.MP => p is Character c && Math.Sign(c.MP - Value) == Sign,
                ConditionType.HPLost => p is Character c && Math.Sign(c.Card.MaxHP - c.HP - Value) == Sign,
                ConditionType.MPLost => p is Character c && Math.Sign(c.Card.MaxMP - c.MP - Value) == Sign,
                ConditionType.Counter => Math.Sign((p is Character c ? (
                    (s is IMaySkillSupplier ssp && ssp.MaySkill is AbstractTriggerable skilltriggerable && c.SkillCounter.TryGetValue(skilltriggerable.NameID, out var counter)) ? counter : c.SkillCounter.ElementAtOrDefault(Index).Value
                    ) : p.AvailableTimes) - Value) == Sign,
                ConditionType.DataCount => Math.Sign(p.Data.Count - Value) == Sign,
                ConditionType.DataContains => p.Data.Contains(Value),
                ConditionType.Region => Math.Sign(p.PersistentRegion - Value) == Sign,
                _ => base.GetPredicate(me, p, s, v)
            };
        }
    }
}