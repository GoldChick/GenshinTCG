namespace TCGBase
{
    public record class ActionRecordDice : ActionRecordBaseWithTeam
    {
        public List<CostRecord> Dice { get; }
        public bool Gain { get; }
        public ActionRecordDice(List<CostRecord> dice, bool gain = true, DamageTargetTeam team = DamageTargetTeam.Me, List<TargetRecord>? when = null) : base(TriggerType.Dice, team, when)
        {
            Dice = dice;
            Gain = gain;
        }
        public override EventPersistentHandler? GetHandler(AbstractTriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                var team = Team == DamageTargetTeam.Enemy ? me.Enemy : me;
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
            };
        }
    }
}
