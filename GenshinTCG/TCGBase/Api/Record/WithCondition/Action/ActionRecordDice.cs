namespace TCGBase
{
    public record class ActionRecordDice : ActionRecordBaseWithTeam
    {
        public List<SingleCostVariable> Dice { get; }
        public bool Gain { get; }
        public ActionRecordDice(List<SingleCostVariable> dice, bool gain = true, TargetTeam team = TargetTeam.Me, List<ConditionRecordBase>? when = null) : base(TriggerType.Dice, team, when)
        {
            Dice = dice;
            Gain = gain;
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            var team = Team == TargetTeam.Enemy ? me.Enemy : me;
            if (Gain)
            {
                foreach (var record in Dice)
                {
                    team.GainDice(record.Type, record.Count);
                }
            }
            else
            {
                throw new NotImplementedException("ActionRecordDice:失去骰子还没做");
            }
        }
    }
}
