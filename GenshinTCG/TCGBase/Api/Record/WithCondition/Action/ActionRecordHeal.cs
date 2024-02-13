
namespace TCGBase
{
    public record class ActionRecordHeal : ActionRecordBase
    {
        public HealRecord Heal { get; }

        public ActionRecordHeal(HealRecord heal, List<List<ConditionRecordBase>>? whenany) : base(TriggerType.Heal, whenany)
        {
            Heal = heal;
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            //TODO: check it
            me.Heal(triggerable, new HealVariable(Heal.Amount, Heal.TargetIndexOffset, Heal.TargetArea == DamageTargetArea.TargetOnly));
        }
    }
}
