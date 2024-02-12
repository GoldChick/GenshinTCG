using System.Text.Json.Serialization;

namespace TCGBase
{
    /// <summary>
    /// 用于Select时，Type只能为Character，Summon，Support
    /// </summary>
    public record class SelectRecord
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TargetType Type { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DamageTargetTeam Team { get; }
        public List<ConditionRecordBase> With { get; }

        public SelectRecord(TargetType type, DamageTargetTeam team = DamageTargetTeam.Me, List<ConditionRecordBase>? with = null)
        {
            Type = type;
            Team = team;
            With = with ?? new();
        }
        public int GetIndexs()
        {
            return 0;
        }
    }
}
