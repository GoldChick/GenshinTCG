using System.Text.Json.Serialization;

namespace TCGBase
{
    /// <summary>
    /// 用于Select时，Type只能为Character，Summon，Support
    /// </summary>
    public record class SelectRecord : IWhenAnyThenAction
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TargetType Type { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DamageTargetTeam Team { get; }
        public List<List<ConditionRecordBase>> WhenAny { get; }

        public SelectRecord(TargetType type, DamageTargetTeam team = DamageTargetTeam.Me, List<List<ConditionRecordBase>>? whenany = null)
        {
            Type = type;
            Team = team;
            WhenAny = whenany ?? new();
        }
        public int GetIndexs()
        {
            return 0;
        }
    }
}
