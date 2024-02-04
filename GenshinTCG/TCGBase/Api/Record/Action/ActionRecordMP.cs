namespace TCGBase
{
    public record class ActionRecordMP : ActionRecordBaseWithTarget
    {
        public int Amount { get; }
        public ActionRecordMP(int amount, DamageTargetTeam team = DamageTargetTeam.Me, CharacterTargetRecord? target = null) : base(TriggerType.MP, team, target)
        {
            Amount = amount;
        }
        public override EventPersistentHandler? GetHandler(ITriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                var team = Team == DamageTargetTeam.Enemy ? me.Enemy : me;
                Target.GetCharacters(team).ForEach(index => team.Characters[index].MP += Amount);
            };
        }
    }
}
