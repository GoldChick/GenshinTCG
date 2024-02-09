using System.Text.Json.Serialization;

namespace TCGBase
{
    public record DamageRecord
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DamageElement Element { get; }
        public int Amount { get; }
        public int TargetIndexOffset { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DamageTargetArea TargetArea { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DamageTargetTeam TargetTeam { get; }
        public DamageRecord? SubDamage { get; internal set; }
        public DamageRecord(DamageElement element, int amount, int targetIndexOffset = 0, DamageTargetArea targetArea = DamageTargetArea.TargetOnly, DamageTargetTeam targetTeam = DamageTargetTeam.Enemy, DamageRecord? subDamage = null)
        {
            Element = element;
            Amount = amount;
            TargetIndexOffset = targetIndexOffset;
            TargetArea = targetArea;
            TargetTeam = targetTeam;
            SubDamage = subDamage;
        }
    }
}
