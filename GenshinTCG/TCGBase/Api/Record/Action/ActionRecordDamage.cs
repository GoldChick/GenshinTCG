namespace TCGBase
{
    public record class ActionRecordDamage : ActionRecordBase
    {
        public DamageRecord Damage { get; }
        public List<ActionRecordBase> With { get; }
        public ActionRecordDamage(DamageRecord damage, List<ActionRecordBase>? with = null) : base(TriggerType.Damage)
        {
            Damage = damage;
            With = with ?? new();
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
                me.DoDamage(new(Damage), triggerable, () => subhandler?.Invoke(me, p, s, v));
            };
        }
    }
}
