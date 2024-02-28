using System.Text.Json.Serialization;

namespace TCGBase
{
    public record class DamageRecord
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DamageElement Element { get; }
        public int Amount { get; }
        public int TargetIndexOffset { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TargetArea TargetArea { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TargetTeam Team { get; }
        public DamageRecord? SubDamage { get;}
        public DamageRecord(DamageElement element, int amount, int targetIndexOffset = 0, TargetArea targetArea = TargetArea.TargetOnly, TargetTeam team = TargetTeam.Enemy, DamageRecord? subDamage = null)
        {
            Element = element;
            Amount = amount;
            TargetIndexOffset = targetIndexOffset;
            TargetArea = targetArea;
            Team = team;
            SubDamage = subDamage;
        }
    }
}
