using System.Text.Json.Serialization;

namespace TCGBase
{
    /// <summary>
    /// 普通攻击，1元素骰2杂色骰，造成2物理伤害/1对应元素伤害
    /// </summary>
    public record TriggerableRecordSkillPreset : TriggerableRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ElementCategory Element { get; }
        public bool DoElementDamage { get; }
        public string CardName { get; }
        public TriggerableRecordSkillPreset(ElementCategory element, string? cardname = null, bool doelementdamage = false) : base(TriggerableType.Skill_A)
        {
            Element = element;
            DoElementDamage = doelementdamage;
            CardName = cardname ?? "skill";
        }
        public override AbstractTriggerable GetTriggerable()
        {
            return new TriggerableRecordSkill.TriggerableSkill(Type switch
            {
                TriggerableType.Skill_E => new TriggerableRecordSkill(
                SkillCategory.E,
                new() { new(Element, 3) },
                new() { new ActionRecordDamage(new((DamageElement)Element, 3)) },
                CardName),

                _ => new TriggerableRecordSkill(
                SkillCategory.A,
                new() { new(ElementCategory.Void, 2), new(Element, 1) },
                new() { new ActionRecordDamage(new(DoElementDamage ? (DamageElement)Element : DamageElement.Trivial, DoElementDamage ? 1 : 2)) },
                CardName)
            });
        }
    }
}
