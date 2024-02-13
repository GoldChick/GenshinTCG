using System.Text.Json.Serialization;

namespace TCGBase
{
    /// <summary>
    /// 普通攻击，1元素骰2杂色骰，造成2物理伤害/1对应元素伤害
    /// </summary>
    public record TriggerableRecordSkill_A : TriggerableRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ElementCategory Element { get; }
        public bool DoElementDamage { get; }

        public TriggerableRecordSkill_A(ElementCategory element, bool doelementdamage = false) : base(TriggerableType.Skill_A)
        {
            Element = element;
            DoElementDamage = doelementdamage;
        }
        public override AbstractTriggerable GetTriggerable()
        {
            return new TriggerableSkill(new TriggerableRecordSkill(
                SkillCategory.A,
                new() { new(ElementCategory.Void, 2), new(Element, 1) },
                new() { new ActionRecordDamage(new(DoElementDamage ? (DamageElement)Element : DamageElement.Trival, DoElementDamage ? 1 : 2)) })
                );
        }
    }
}
