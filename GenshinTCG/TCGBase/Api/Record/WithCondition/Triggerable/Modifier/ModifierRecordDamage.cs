using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum ModifierDamageMode
    {
        Add,
        Mul,
        DivideCeil,
        DivideFloor,
        DivideRound,
    }
    public record class ModifierRecordDamage : ModifierRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModifierDamageMode Mode { get; }
        public ModifierRecordDamage(int value, ModifierDamageMode mode = ModifierDamageMode.Add, int consume = 1, List<ConditionRecordBase>? when = null, ActionRecordTrigger? trigger = null) : base(ModifierType.Damage, value, consume, when, trigger)
        {
            Mode = mode;
        }
        protected override string GetSenderName()
        {
            return (Mode == ModifierDamageMode.Add ? SenderTag.DamageIncrease : SenderTag.DamageMul).ToString();
        }
        protected override EventPersistentHandler? Get()
        {
            return (me, p, s, v) =>
            {
                if (v is DamageVariable dv)
                {
                    switch (Mode)
                    {
                        case ModifierDamageMode.Add:
                            dv.Amount += Value;
                            break;
                        case ModifierDamageMode.Mul:
                            dv.Amount *= Value;
                            break;
                        case ModifierDamageMode.DivideCeil:
                            dv.Amount = (Value + 1) / 2;
                            break;
                        case ModifierDamageMode.DivideFloor:
                            dv.Amount = Value / 2;
                            break;
                        case ModifierDamageMode.DivideRound:
                            dv.Amount = (int)Math.Round(((double)Value) / 2);
                            break;
                    }
                    p.AvailableTimes -= Consume;
                }
            };
        }
    }
}
