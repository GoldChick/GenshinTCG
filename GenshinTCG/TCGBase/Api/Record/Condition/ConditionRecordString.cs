using System.Text.Json;

namespace TCGBase
{
    public record class ConditionRecordString : ConditionRecordBase
    {
        public string Value { get; }
        public ConditionRecordString(ConditionType type, string? value = null, bool not = false) : base(type, not)
        {
            Value = value ?? "test";
        }
        protected override bool GetPredicate(PlayerTeam me, Persistent? p, AbstractSender? s, AbstractVariable? v)
        {
            return Type switch
            {
                ConditionType.Element => v is DamageVariable dv && dv.Element.ToString() == Value,
                ConditionType.Reaction => v is DamageVariable dv && dv.Reaction.ToString() == Value,
                ConditionType.SkillType => s is HurtSourceSender hss && hss.Triggerable is ISkillable skill && skill.SkillCategory.ToString() == Value,
                ConditionType.Related => v is DamageVariable dv && (((DamageElement)((int)dv.Reaction / 10)).ToString() == Value || ((DamageElement)((int)dv.Reaction % 10)).ToString() == Value),
                //throw new Exception($"{JsonSerializer.Serialize((p as Character).Effects._data)} and need is {Value}"),//&&
                ConditionType.HasEffect =>  p is Character c &&  c.Effects.Contains(Value),
                ConditionType.HasEffectWithTag => p is Character c && c.Effects.Contains(ef => ef.CardBase.Tags.Contains(Value)),
                ConditionType.HasTag => p != null && p.CardBase.Tags.Contains(Value),
                ConditionType.Name => $"{p?.CardBase.Namespace}:{p?.CardBase.NameID}" == Value,
                _ => base.GetPredicate(me, p, s, v)
            };
        }
    }
}