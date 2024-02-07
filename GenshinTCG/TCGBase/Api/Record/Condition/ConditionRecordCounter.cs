using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum CardCounterType
    {
        Counter,
        HP,
        MP,
    }
    public record class ConditionRecordCounter : ConditionRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CardCounterType CounterType { get; }
        public int Above { get; }
        public int Below { get; }
        public bool Full { get; }
        public int Skill { get; }

        public ConditionRecordCounter(CardCounterType counterType, int above = 0, int below = 114, bool full = false, int skill = -1) : base(ConditionType.Counter)
        {
            CounterType = counterType;
            Above = above;
            Below = below;
            Full = full;
            Skill = skill;
        }
    }
}
