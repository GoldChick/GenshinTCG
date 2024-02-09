using System.Text.Json.Serialization;

namespace TCGBase
{
    public record HealRecord
    {
        public int Amount { get; }
        public int TargetIndexOffset { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DamageTargetArea TargetArea { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DamageTargetTeam TargetTeam { get; }
        public HealRecord? SubHeal { get; internal set; }
        public HealRecord(int amount, int targetIndexOffset = 0, DamageTargetArea targetArea = DamageTargetArea.TargetOnly, DamageTargetTeam targetTeam = DamageTargetTeam.Me, HealRecord? subHeal = null)
        {
            Amount = amount;
            TargetIndexOffset = targetIndexOffset;
            TargetArea = targetArea;
            TargetTeam = targetTeam;
            SubHeal = subHeal;
        }
    }
}
