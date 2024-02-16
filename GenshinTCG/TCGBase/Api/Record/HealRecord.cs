using System.Text.Json.Serialization;

namespace TCGBase
{
    public record HealRecord
    {
        public int Amount { get; }
        public int TargetIndexOffset { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TargetArea TargetArea { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TargetTeam TargetTeam { get; }
        public HealRecord? SubHeal { get; internal set; }
        public HealRecord(int amount, int targetIndexOffset = 0, TargetArea targetArea = TargetArea.TargetOnly, TargetTeam targetTeam = TargetTeam.Me, HealRecord? subHeal = null)
        {
            Amount = amount;
            TargetIndexOffset = targetIndexOffset;
            TargetArea = targetArea;
            TargetTeam = targetTeam;
            SubHeal = subHeal;
        }
    }
}
