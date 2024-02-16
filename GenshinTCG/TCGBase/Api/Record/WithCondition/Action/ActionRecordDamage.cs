namespace TCGBase
{
    public record class ActionRecordDamage : ActionRecordBase
    {
        public DamageRecord Damage { get; }
        public List<ActionRecordBase> With { get; }
        public ActionRecordDamage(DamageRecord damage, List<ActionRecordBase>? with = null, List<ConditionRecordBase>? when = null) : base(TriggerType.Damage, when)
        {
            Damage = damage;
            With = with ?? new();
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            EventPersistentHandler? subhandler = null;
            foreach (ActionRecordBase action in With)
            {
                subhandler += action.GetHandler(triggerable);
            }
            me.DoDamage(new(Damage), p, triggerable, () =>
            {
                subhandler?.Invoke(me, p, s, v);
            });
        }
    }
}
