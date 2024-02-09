namespace TCGBase
{
    public record class ActionRecordDice : ActionRecordBaseWithTeam
    {
        public List<CostRecord> Dice { get; }
        public bool Gain { get; }
        public ActionRecordDice(List<CostRecord> dice, bool gain = true, DamageTargetTeam team = DamageTargetTeam.Me) : base(TriggerType.MP, team)
        {
            Dice = dice;
            Gain = gain;
        }
        public override EventPersistentHandler? GetHandler(ITriggerable triggerable)
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
