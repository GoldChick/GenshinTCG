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
    /// <summary>
    /// target得的count，可设定是否*倍率、*可用次数、乘完再加多少
    /// </summary>
    public record class DamageDynamicValue
    {
        public TargetRecord Target { get; }
        public bool MulAvailableTimes { get; }
        public int Mul { get; }
        public int OffSet { get; }
        public DamageDynamicValue(TargetRecord? target, bool mulAvailableTimes = false, int mul = 1, int offset = 0)
        {
            Target = target ?? new();
            MulAvailableTimes = mulAvailableTimes;
            Mul = mul;
            OffSet = offset;
        }
        public int GetValue(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            var targets = Target.GetTargets(me, p, s, v, out _);
            if (MulAvailableTimes)
            {
                return OffSet + targets.Sum(t => t.AvailableTimes) * Mul;
            }
            else
            {
                return OffSet + targets.Count * Mul;
            }
        }
    }
    public record class ModifierRecordDamage : ModifierRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModifierDamageMode Mode { get; }
        public DamageDynamicValue? DynamicValue { get; }
        public ModifierRecordDamage(int value, DamageDynamicValue? dynamicvalue = null, int adddata = -1, ModifierDamageMode mode = ModifierDamageMode.Add, int consume = 1, List<ConditionRecordBase>? when = null, ActionRecordTrigger? trigger = null) : base(ModifierType.Damage, value, consume, adddata, when, trigger)
        {
            Mode = mode;
            DynamicValue = dynamicvalue;
        }
        protected override string GetSenderName()
        {
            return (Mode == ModifierDamageMode.Add ? SenderTag.DamageIncrease : SenderTag.DamageMul).ToString();
        }
        protected override EventPersistentHandler? Get(Action whensuccess)
        {
            return (me, p, s, v) =>
            {
                int value = DynamicValue?.GetValue(me, p, s, v) ?? Value;

                if (v is DamageVariable dv)
                {
                    switch (Mode)
                    {
                        case ModifierDamageMode.Add:
                            dv.Amount += value;
                            break;
                        case ModifierDamageMode.Mul:
                            dv.Amount *= value;
                            break;
                        case ModifierDamageMode.DivideCeil:
                            dv.Amount = (value + 1) / 2;
                            break;
                        case ModifierDamageMode.DivideFloor:
                            dv.Amount = value / 2;
                            break;
                        case ModifierDamageMode.DivideRound:
                            dv.Amount = (int)Math.Round(((double)value) / 2);
                            break;
                    }
                    p.AvailableTimes -= Consume;
                    whensuccess.Invoke();
                }
            };
        }
    }
}
