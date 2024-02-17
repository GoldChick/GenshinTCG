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
        public TargetTeam Team { get; }
        public HealRecord? SubHeal { get; internal set; }
        public HealRecord(int amount, int targetIndexOffset = 0, TargetArea targetArea = TargetArea.TargetOnly, TargetTeam team = TargetTeam.Me, HealRecord? subHeal = null)
        {
            Amount = amount;
            TargetIndexOffset = targetIndexOffset;
            TargetArea = targetArea;
            Team = team;
            SubHeal = subHeal;
        }
    }
}
