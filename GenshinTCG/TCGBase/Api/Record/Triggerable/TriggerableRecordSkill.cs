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
        public List<CostRecord> Cost { get; }
        public List<ActionRecordBase> Action { get; }

        public TriggerableRecordSkill(SkillCategory category, List<CostRecord> cost, List<ActionRecordBase> action) : base(TriggerableType.Skill)
        {
            Category = category;
            Cost = cost;
            Action = action;
        }
        public override ITriggerable GetTriggerable() => new TriggerableSkill(this);
    }
}
