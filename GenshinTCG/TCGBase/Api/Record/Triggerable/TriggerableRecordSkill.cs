using System.Text.Json.Serialization;

namespace TCGBase
{
    /// <summary>
    /// 主动技能
    /// </summary>
    public record TriggerableRecordSkill : TriggerableRecordWithAction
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SkillCategory Category { get; }
        public List<CostRecord> Cost { get; }
        public int MP { get; }
        public TriggerableRecordSkill(SkillCategory category, List<CostRecord> cost, List<ActionRecordBase> action, int mp = 1) : base(TriggerableType.Skill, action)
        {
            Category = category;
            Cost = cost;
            MP = mp;
        }
        public override AbstractTriggerable GetTriggerable() => new TriggerableSkill(this);
    }
}
