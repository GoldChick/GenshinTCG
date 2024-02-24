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
                ConditionType.Element => v is ElementVariable ev && (Enum.TryParse(Value, true, out DamageElement de) ? ev.Element == de : ev.Element is not (DamageElement.Trival or DamageElement.Pierce)),
                ConditionType.ElementReaction => v is ElementVariable ev && (Enum.TryParse(Value, true, out ReactionTags reaction) ? ev.Reaction == reaction : ev.Reaction is not ReactionTags.None),
                ConditionType.SkillType => s is IMaySkillSupplier ssp && ssp.MaySkill is ISkillable skill && !(Enum.TryParse(Value, true, out SkillCategory skilltype) && skill.SkillCategory != skilltype),
                ConditionType.ElementRelated => v is DamageVariable dv && (((DamageElement)((int)dv.Reaction / 10)).ToString() == Value || ((DamageElement)((int)dv.Reaction % 10)).ToString() == Value),
                ConditionType.ThisCharacterCause => v is ElementVariable dv && dv.Direct == DamageSource.Direct && s.TeamID == me.TeamIndex && s is IPeristentSupplier ips && ips.Persistent is Character c && c.PersistentRegion == p?.PersistentRegion && s is IMaySkillSupplier imss && imss.MaySkill is ISkillable skill && !(Enum.TryParse(Value, true, out SkillCategory skilltype) && skill.SkillCategory != skilltype),
                ConditionType.OurCharacterCause => v is ElementVariable dv && dv.Direct == DamageSource.Direct && s.TeamID == me.TeamIndex && s is IMaySkillSupplier imss && imss.MaySkill is ISkillable skill && !(Enum.TryParse(Value, true, out SkillCategory skilltype) && skill.SkillCategory != skilltype),
                //throw new Exception($"{JsonSerializer.Serialize((p as Character).Effects._data)} and need is {Value}"),//&&
                ConditionType.HasEffect => p is Character c && c.Effects.Contains(Value),
                ConditionType.HasEffectWithTag => p is Character c && c.Effects.Contains(ef => ef.CardBase.Tags.Contains(Value)),
                ConditionType.HasTag => p.CardBase.Tags.Contains(Value),
                ConditionType.Name => $"{p.CardBase.Namespace}:{p.CardBase.NameID}" == Value,
                ConditionType.SimpleTalent => p.PersistentRegion == me.CurrCharacter && $"{p.CardBase.Namespace}:{p.CardBase.NameID}" == Value && p is Character c && !c.Effects.Contains(ef => ef.CardBase.Tags.Contains("AntiSkill")) && c.Alive,
                ConditionType.SimpleWeapon => p is Character c && Enum.TryParse(Value, true, out WeaponCategory _) && c.CardBase.Tags.Contains(Value),
                _ => base.GetPredicate(me, p, s, v)
            };
        }
    }
}