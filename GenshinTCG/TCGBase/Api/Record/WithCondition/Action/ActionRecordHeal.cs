
namespace TCGBase
{
    public record class ActionRecordHeal : ActionRecordBase
    {
        public HealRecord Heal { get; }

        public ActionRecordHeal(HealRecord heal, List<ConditionRecordBase>? when) : base(TriggerType.Heal, when)
        {
            Heal = heal;
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            me.DoHeal(Heal, p, triggerable);
        }
    }
}
