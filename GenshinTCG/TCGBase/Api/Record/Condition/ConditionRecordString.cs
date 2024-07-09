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
                ConditionType.ElementRelated => v is DamageVariable dv && (((DamageElement)((int)dv.Reaction / 10)).ToString() == Value || ((DamageElement)((int)dv.Reaction % 10)).ToString() == Value),
                ConditionType.ThisCharacterCause => v is ElementVariable dv && dv.Direct == DamageSource.Direct && s.TeamID == me.TeamID && s is IPeristentSupplier ips && ips.Persistent is Character c && c.PersistentRegion == p?.PersistentRegion && s is IMaySkillSupplier imss && imss.MaySkill is ISkillable skill && !(Enum.TryParse(Value, true, out SkillCategory skilltype) && skill.SkillCategory != skilltype),
                ConditionType.OurCharacterCause => v is ElementVariable dv && dv.Direct == DamageSource.Direct && s.TeamID == me.TeamID && s is IMaySkillSupplier imss && imss.MaySkill is ISkillable skill && !(Enum.TryParse(Value, true, out SkillCategory skilltype) && skill.SkillCategory != skilltype),
                ConditionType.HasEffectWithTag => p is Character c && c.Effects.Contains(ef => ef.CardBase.Tags.Contains(Value)),
                ConditionType.HasCard => me.CardsInHand.Any(p => $"{p.CardBase.Namespace}:{p.CardBase.NameID}" == Value),
                ConditionType.OperationType => s is AfterOperationSender os && !(Enum.TryParse(Value, true, out OperationType type) && os.ActionType != type),
                ConditionType.SimpleTalent => p.PersistentRegion == me.CurrCharacter && $"{p.CardBase.Namespace}:{p.CardBase.NameID}" == Value && p is Character c && !c.Effects.Contains(ef => ef.CardBase.Tags.Contains("AntiSkill")) && c.Alive,
                ConditionType.SimpleWeapon => p is Character c && Enum.TryParse(Value, true, out WeaponCategory _) && c.CardBase.Tags.Contains(Value),
                _ => base.GetPredicate(me, p, s, v)
            };
        }
    }
}