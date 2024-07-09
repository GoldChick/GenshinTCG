using System.Text.Json.Serialization;

namespace TCGBase
{
    /// <summary>
    /// 用于Select时，Type只能为Character，Summon，Support，还有Lua
    /// </summary>
    public record class SelectRecord : IWhenThenAction
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TargetType Type { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TargetTeam Team { get; }
        public List<ConditionRecordBase> When { get; }

        public SelectRecord(TargetType type, TargetTeam team = TargetTeam.Me, List<ConditionRecordBase>? when = null)
        {
            Type = type;
            Team = team;
            When = when ?? new();
        }
    }
}
