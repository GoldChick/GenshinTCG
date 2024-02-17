using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum ModifierType
    {
        Dice,
        Damage,
        Element,
        //下面两个内置 When TargetMe
        Shield,
        Barrier
    }
    public enum ModifierMode
    {
        Add,
        Mul,
        DivideCeil,
        DivideFloor,
        DivideRound,
    }
    public record class ModifierRecordBase : IWhenThenAction
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModifierType Type { get; }
        //只对Damage有效
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModifierMode Mode { get; }
        public int Value { get; }
        /// <summary>
        /// 如果成功触发，减少多少AvailableTime，默认1
        /// </summary>
        public int Consume { get; }
        public List<ConditionRecordBase> When { get; }

        public ModifierRecordBase(ModifierType type, int value, ModifierMode mode = ModifierMode.Add, int consume = 1, List<ConditionRecordBase>? when = null)
        {
            Type = type;
            Mode = mode;
            Value = int.Min(value, 1);
            Consume = consume;
            When = when ?? new();
            switch (Type)
            {
                case ModifierType.Shield:
                case ModifierType.Element:
                case ModifierType.Barrier:
                    Consume = 0;
                    break;
            }
        }
        public virtual AbstractTriggerable GetTriggerable()
        {
            return new Triggerable((Type switch
            {
                ModifierType.Damage => Mode == ModifierMode.Add ? SenderTag.DamageIncrease : SenderTag.DamageMul,
                ModifierType.Element => SenderTag.ElementEnchant,
                ModifierType.Shield or ModifierType.Barrier => SenderTag.HurtDecrease,
                _ => throw new NotImplementedException($"UnImplemented Modifier Record Type: {Type}")
            }).ToString(), GetHandler());
        }
        public EventPersistentHandler GetHandler()
        {
            return (me, p, s, v) =>
            {
                if ((this as IWhenThenAction).IsConditionValid(me, p, s, v))
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
                ConditionRecordBase targetme = new(ConditionType.TargetMe, false, null);

                if (v is DamageVariable dv)
                {
                    switch (Type)
                    {
                        case ModifierType.Dice:
                            //TODO:
                            break;
                        case ModifierType.Damage:
                            switch (Mode)
                            {
                                case ModifierMode.Add:
                                    dv.Amount += Value;
                                    break;
                                case ModifierMode.Mul:
                                    dv.Amount *= Value;
                                    break;
                                case ModifierMode.DivideCeil:
                                    dv.Amount = (Value + 1) / 2;
                                    break;
                                case ModifierMode.DivideFloor:
                                    dv.Amount = Value / 2;
                                    break;
                                case ModifierMode.DivideRound:
                                    dv.Amount = (int)Math.Round(((double)Value) / 2);
                                    break;
                            }
                            break;
                        case ModifierType.Element:
                            if (dv.Element == DamageElement.Trival)
                            {
                                dv.Element = (DamageElement)Value;
                                p.AvailableTimes -= 1;
                            }
                            break;
                        case ModifierType.Shield:
                            if (targetme.Valid(me, p, s, v))
                            {
                                var min = int.Min(p.AvailableTimes, dv.Amount);
                                dv.Amount -= min;
                                p.AvailableTimes -= min;
                            }
                            break;
                        case ModifierType.Barrier:
                            if (targetme.Valid(me, p, s, v) && dv.Amount > 0)
                            {
                                dv.Amount -= Value;
                                p.AvailableTimes--;
                            }
                            break;
                    }
                }
            };
        }
    }
}
