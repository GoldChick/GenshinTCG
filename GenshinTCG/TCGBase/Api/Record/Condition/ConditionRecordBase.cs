using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum ConditionType
    {
        Character,
        Skill,
        Effect,
        Summon,
        Support,
        Card
    }
    public enum ConditionTypeX
    {
        /// <summary>
        /// 状态/技能的目前可用次数为x时
        /// </summary>
        CounterEquals,
        /// <summary>
        /// 该技能对应的天赋已装备时
        /// </summary>
        TalentEquiped,
    }
    public record class ConditionRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ConditionType Type;

        public ConditionRecordBase(ConditionType type)
        {
            Type = type;
        }
    }
}
