
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
                case ConditionType.Damage:
                    if (v is DamageVariable dv)
                    {
                        value = dv.Amount;
                        return true;
                    }
                    break;
                case ConditionType.HP:
                    if (me.Characters.ElementAtOrDefault(p.PersistentRegion) is Character c)
                    {
                        value = c.HP;
                        return true;
                    }
                    break;
                case ConditionType.MP:
                    if (me.Characters.ElementAtOrDefault(p.PersistentRegion) is Character c1)
                    {
                        value = c1.HP;
                        return true;
                    }
                    break;
                case ConditionType.MPLost:
                    if (me.Characters.ElementAtOrDefault(p.PersistentRegion) is Character c2)
                    {
                        value = c2.Card.MaxMP - c2.MP;
                        return true;
                    }
                    break;
                case ConditionType.HPLost:
                    if (me.Characters.ElementAtOrDefault(p.PersistentRegion) is Character c3)
                    {
                        value = c3.Card.MaxHP - c3.HP;
                        return true;
                    }
                    break;
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
                case ConditionType.DataCount:
                    value = p.Data.Count;
                    return true;
                case ConditionType.Region:
                    value = p.PersistentRegion;
                    return true;
            }
            return false;
        }
        protected override bool GetPredicate(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            int destinationValue = (DynamicValue?.GetNum(me, p, s, v, out var dynamicvalue) ?? false) ? dynamicvalue : Value;

            return Type switch
            {
                ConditionType.DataContains => p.Data.Contains(destinationValue),
                //ConditionType.Damage or ConditionType.SkillCostSum or
                //ConditionType.HP or ConditionType.MP or ConditionType.HPLost or ConditionType.MPLost or ConditionType.Counter or
                //ConditionType.DataCount or ConditionType.Region
                _ => GetNum(me, p, s, v, out var currValue) && Math.Sign(currValue - destinationValue) == Sign
            };
        }
    }
}