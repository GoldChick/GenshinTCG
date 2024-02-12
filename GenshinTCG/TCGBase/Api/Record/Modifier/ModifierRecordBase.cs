using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum ModifierType
    {
        Dice,
        Damage
    }
    public enum ModifierComputeType
    {
        Add,
        Sub,
        Mul,
        Divide,
        Set,
    }

    public record class ModifierRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModifierType Type { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModifierComputeType Compute { get; }
        public int Value { get; }
        public List<TargetRecord> When { get; }

        public ModifierRecordBase(ModifierType type, ModifierComputeType compute, int value, List<TargetRecord>? when)
        {
            Type = type;
            Compute = compute;
            Value = value;
            When = when ?? new();
        }
        public virtual EventPersistentHandler? Get()
        {
            return null;
        }
    }
}
