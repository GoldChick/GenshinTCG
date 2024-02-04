using System.Text.Json.Serialization;

namespace TCGBase
{
    public record CostRecord
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ElementCategory Type { get; }
        public int Count { get; }
        public CostRecord(ElementCategory type, int count)
        {
            Type = type;
            Count = count;
        }
    }
}
