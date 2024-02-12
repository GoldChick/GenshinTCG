using System.Text.Json.Serialization;

namespace TCGBase
{
    /// <summary>
    /// 主动技能
    /// </summary>
    public record TriggerableRecordSkill_E : TriggerableRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ElementCategory Element { get; }
        public bool DoElementDamage { get; }

        public TriggerableRecordSkill_E(ElementCategory element, bool doelementdamage = true) : base(TriggerableType.Skill_E)
        {
            Element = element;
            DoElementDamage = doelementdamage;
        }
        public override AbstractCustomTriggerable GetTriggerable()
        {
            return new TriggerableSkill(new TriggerableRecordSkill(
                SkillCategory.E,
                new() { new(Element, 3) },
                new() { new ActionRecordDamage(new((DamageElement)Element, 3)) })
                );
        }
    }
}
