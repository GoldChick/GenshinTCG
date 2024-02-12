
namespace TCGBase
{
    public record class ActionRecordHeal : ActionRecordBase
    {
        public HealRecord Heal { get; }

        public ActionRecordHeal(HealRecord heal, List<TargetRecord>? when) : base(TriggerType.Heal, when)
        {
            Heal = heal;
        }
        public override EventPersistentHandler? GetHandler(AbstractCustomTriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                //TODO: check it
                me.Heal(triggerable, new HealVariable(Heal.Amount, Heal.TargetIndexOffset, Heal.TargetArea == DamageTargetArea.TargetOnly));
            };
        }
    }
}
