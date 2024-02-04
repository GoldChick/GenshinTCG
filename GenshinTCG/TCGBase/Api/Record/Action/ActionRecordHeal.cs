
namespace TCGBase
{
    public record class ActionRecordHeal : ActionRecordBase
    {
        public HealRecord Heal { get; }

        public ActionRecordHeal(HealRecord heal) : base(TriggerType.Heal)
        {
            Heal = heal;
        }
        public override EventPersistentHandler? GetHandler(ITriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                //TODO: check it
                me.Heal(triggerable, new HealVariable(Heal.Amount, Heal.TargetIndexOffset, Heal.TargetArea == DamageTargetArea.TargetOnly));
            };
        }
    }
}
