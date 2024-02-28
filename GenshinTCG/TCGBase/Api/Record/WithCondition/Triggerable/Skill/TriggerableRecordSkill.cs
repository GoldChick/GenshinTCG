using System.Text.Json.Serialization;

namespace TCGBase
{
    /// <summary>
    /// 主动技能
    /// </summary>
    public record TriggerableRecordSkill : TriggerableRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SkillCategory Category { get; }
        public List<SingleCostVariable> Cost { get; }
        public int MP { get; }
        public string CardName { get; }
        public TriggerableRecordSkill(SkillCategory category, List<SingleCostVariable> cost, List<ActionRecordBase> action, string? cardname = null, int mp = 1, List<ConditionRecordBase>? when = null) : base(TriggerableType.Skill, action, when)
        {
            Category = category;
            Cost = cost;
            MP = mp;
            CardName = cardname ?? "skill";
        }
        public override AbstractTriggerable GetTriggerable() => new TriggerableSkill(this);
    }
}
