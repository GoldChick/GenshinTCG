using System.Text.Json;

namespace TCGBase
{
    public record class ActionRecordDamage : ActionRecordBase
    {
        public DamageRecord Damage { get; }
        public List<ActionRecordBase> With { get; }
        public ActionRecordDamage(DamageRecord damage, List<ActionRecordBase>? with = null, List<TargetRecord>? when = null) : base(TriggerType.Damage, when)
        {
            Damage = damage;
            With = with ?? new();
        }
        public override EventPersistentHandler? GetHandler(AbstractCustomTriggerable triggerable)
        {
            EventPersistentHandler? subhandler = null;
            foreach (ActionRecordBase action in With)
            {
                subhandler += action.GetHandler(triggerable);
            }
            return (me, p, s, v) =>
            {
                me.DoDamage(new(Damage), triggerable, () =>
                {
                    subhandler?.Invoke(me, p, s, v);
                });
            };
        }
    }
}
