
namespace TCGBase
{
    public record class ConditionRecordTriInt : ConditionRecordBase
    {
        public int Sign { get; }
        public int Index { get; }
        public int Value { get; }
        /// <summary>
        /// 自动调用GetNum()，作为比较的减数
        /// </summary>
        public ConditionRecordTriInt? DynamicValue { get; }
        public ConditionRecordTriInt(ConditionType type, int value, int index = -1, int sign = 0, bool not = false, ConditionRecordTriInt? dynamicvalue = null, ConditionRecordBase? or = null) : base(type, not, or)
        {
            Index = index;
            Value = value;
            Sign = Math.Sign(sign);
            DynamicValue = dynamicvalue;
        }
        private bool GetNum(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v, out int value)
        {
            value = 0;
            switch (Type)
            {
                case ConditionType.Counter:
                    if (p is Character c4)
                    {
                        if (Index < 0 && s is IMaySkillSupplier ssp1 && ssp1.MaySkill is AbstractTriggerable skilltriggerable && c4.SkillCounter.TryGetValue(skilltriggerable.NameID, out var counter))
                        {
                            value = counter;
                        }
                        else
                        {
                            value = c4.SkillCounter.ElementAtOrDefault(Index).Value;
                        }
                        return true;
                    }
                    value = p.AvailableTimes;
                    return true;
            }
            return false;
        }
        protected override bool GetPredicate(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            int destinationValue = (DynamicValue?.GetNum(me, p, s, v, out var dynamicvalue) ?? false) ? dynamicvalue : Value;

            return Type switch
            {
                _ => GetNum(me, p, s, v, out var currValue) && Math.Sign(currValue - destinationValue) == Sign
            };
        }
    }
}