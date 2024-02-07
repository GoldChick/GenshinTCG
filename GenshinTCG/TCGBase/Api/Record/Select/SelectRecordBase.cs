using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum SelectType
    {
        Character,
        Summon,
        Support,
    }
    public record class SelectRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SelectType Type { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DamageTargetTeam Team { get; }
        public List<ConditionRecordBase> With { get; }
        public List<ConditionRecordBase> Without { get; }

        public SelectRecordBase(SelectType type, DamageTargetTeam team = DamageTargetTeam.Me, List<ConditionRecordBase>? with = null, List<ConditionRecordBase>? without = null)
        {
            Type = type;
            Team = team;
            With = with ?? new();
            Without = without ?? new();
        }
    }
}
