namespace TCGBase
{
    public record class ConditionRecordString : ConditionRecordBase
    {
        public string Value { get; }
        public ConditionRecordString(ConditionType type, string? value = null, bool not = false, ConditionRecordBase? or = null) : base(type, not, or)
        {
            Value = value ?? "test";
        }
        protected override bool GetPredicate(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            return Type switch
            {
                ConditionType.Element => v is DamageVariable dv && dv.Element.ToString() == Value,
                ConditionType.Reaction => v is DamageVariable dv && dv.Reaction.ToString() == Value,
                ConditionType.SkillType => s is IMaySkillSupplier ssp && ssp.MaySkill is ISkillable skill && !(Enum.TryParse(Value, true, out SkillCategory skilltype) && skill.SkillCategory != skilltype),
                ConditionType.ElementRelated => v is DamageVariable dv && (((DamageElement)((int)dv.Reaction / 10)).ToString() == Value || ((DamageElement)((int)dv.Reaction % 10)).ToString() == Value),
                ConditionType.ThisCharacterCause => v is DamageVariable dv && dv.Direct == DamageSource.Direct && s is HurtSourceSender hss && hss.TeamID == me.TeamIndex && hss.Source is Character c && c.PersistentRegion == p?.PersistentRegion && hss.Triggerable is ISkillable skill && !(Enum.TryParse(Value, true, out SkillCategory skilltype) && skill.SkillCategory != skilltype),
                ConditionType.OurCharacterCause => v is DamageVariable dv && dv.Direct == DamageSource.Direct && s is HurtSourceSender hss && hss.TeamID == me.TeamIndex && hss.Triggerable is ISkillable skill && !(Enum.TryParse(Value, true, out SkillCategory skilltype) && skill.SkillCategory != skilltype),
                //throw new Exception($"{JsonSerializer.Serialize((p as Character).Effects._data)} and need is {Value}"),//&&
                ConditionType.HasEffect => p is Character c && c.Effects.Contains(Value),
                ConditionType.HasEffectWithTag => p is Character c && c.Effects.Contains(ef => ef.CardBase.Tags.Contains(Value)),
                ConditionType.HasTag => p != null && p.CardBase.Tags.Contains(Value),
                ConditionType.Name => $"{p?.CardBase.Namespace}:{p?.CardBase.NameID}" == Value,
                ConditionType.SimpleTalent => p?.PersistentRegion == me.CurrCharacter && $"{p?.CardBase.Namespace}:{p?.CardBase.NameID}" == Value && p is Character c && !c.Effects.Contains(ef => ef.CardBase.Tags.Contains("AntiSkill")) && c.Alive,
                ConditionType.SimpleWeapon => p is Character c && Enum.TryParse(Value, true, out WeaponCategory _) && c.CardBase.Tags.Contains(Value),
                _ => base.GetPredicate(me, p, s, v)
            }; 
        }
    }
}