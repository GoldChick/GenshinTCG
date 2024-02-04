namespace TCGBase
{
    public record class ActionRecordDamage : ActionRecordBase
    {
        public DamageRecord Damage { get; }
        public List<ActionRecordBase> With { get; }
        /// <summary>
        /// 对于这个record，CharacterIndexType和DamageTargetTeam无意义
        /// </summary>
        public ActionRecordDamage(DamageRecord damage, List<ActionRecordBase> with) : base(TriggerType.DoDamage)
        {
            Damage = damage;
            With = with;
        }
        public override EventPersistentHandler? GetHandler(ITriggerable triggerable)
        {
            EventPersistentHandler? subhandler = null;
            foreach (ActionRecordBase action in With)
            {
                subhandler += action.GetHandler(triggerable);
            }
            return (me, p, s, v) =>
            {
                me.Enemy.DoDamage(new(Damage), triggerable, () => subhandler?.Invoke(me, p, s, v));
            };
        }
    }
}
