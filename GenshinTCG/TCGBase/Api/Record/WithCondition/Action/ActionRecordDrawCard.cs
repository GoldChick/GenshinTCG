namespace TCGBase
{
    public record class ActionRecordDrawCard : ActionRecordBaseWithTeam
    {
        public int Count { get; }
        public List<string> WithTag { get; }
        public ActionRecordDrawCard(int count = 1, List<string>? withtag = null, TargetTeam team = TargetTeam.Me, List<ConditionRecordBase>? when = null) : base(TriggerType.Dice, team, when)
        {
            Count = count;
            WithTag = withtag ?? new();
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            (Team == TargetTeam.Enemy ? me.Enemy : me).RollCard(Count, c => WithTag.All(c.Tags.Contains));
        }
    }
}
