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

        public SelectRecordBase(SelectType type)
        {
            Type = type;
        }
    }
}
