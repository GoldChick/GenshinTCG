namespace TCGBase
{
    public record class ActionRecordDrawCard : ActionRecordBaseWithTeam
    {
        public int Value { get; }
        public List<string> WithTag { get; }
        public ActionRecordDrawCard(int value = 1, List<string>? withtag = null, TargetTeam team = TargetTeam.Me, List<ConditionRecordBase>? when = null) : base(TriggerType.Dice, team, when)
        {
            Value = value;
            WithTag = withtag ?? new();
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            var team = Team == TargetTeam.Enemy ? me.Enemy : me;
            //TODO: draw card
        }
    }
}
