using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum ModifierType
    {
        Dice,
        Damage,
        Element,
        Shield
    }
    public enum ModifierMode
    {
        Add,
        Mul,
        DivideCeil,
        DivideFloor,
        DivideRound,
    }
    public record class ModifierRecordBase : IWhenAnyThenAction
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModifierType Type { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModifierMode Mode { get; }
        public int Value { get; }
        /// <summary>
        /// 如果成功触发，减少多少AvailableTime
        /// </summary>
        public int Consume { get; }
        public List<List<ConditionRecordBase>> WhenAny { get; }

        public ModifierRecordBase(ModifierType type, int value, ModifierMode mode = ModifierMode.Add, int consume = 1, List<List<ConditionRecordBase>>? whenany = null)
        {
            Type = type;
            Mode = mode;
            Value = value;
            Consume = consume;
            WhenAny = whenany ?? new();
        }
        public virtual AbstractTriggerable GetTriggerable()
        {
            return new Triggerable((Type switch
            {
                ModifierType.Damage => Mode == ModifierMode.Add ? SenderTag.DamageIncrease : SenderTag.DamageMul,
                ModifierType.Element => SenderTag.ElementEnchant,
                _ => throw new NotImplementedException($"UnImplemented Modifier Record Type: {Type}")
            }).ToString(), GetHandler());
        }
        public EventPersistentHandler GetHandler()
        {
            return (me, p, s, v) =>
            {
                if ((this as IWhenAnyThenAction).IsConditionValid(me, p, s, v))
                {
                    Get()?.Invoke(me, p, s, v);
                    p.AvailableTimes -= Consume;
                }
            };
        }
        protected virtual EventPersistentHandler? Get()
        {
            return (me, p, s, v) =>
            {
                if (v is DamageVariable dv)
                {
                    switch (Type)
                    {
                        case ModifierType.Dice:
                            break;
                        case ModifierType.Damage:
                            dv.Damage += Value;
                            break;
                        case ModifierType.Element:
                            dv.Element = (DamageElement)Value;
                            break;
                        case ModifierType.Shield:
                            break;
                        default:
                            break;
                    }
                }
            };
        }
    }
}
