using System.Text.Json.Serialization;

namespace TCGBase
{
    public class SingleCostVariable : AbstractVariable
    {
        private int _count;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ElementCategory Type { get; }
        public int Count { get => _count; set => _count = int.Max(0, value); }

        public SingleCostVariable(ElementCategory type, int count)
        {
            Type = type;
            Count = count;
        }
    }
}
