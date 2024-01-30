namespace TCGBase
{
    /// <summary>
    /// 专为后置结算设计的空team，只有effecttrigger能传出
    /// TODO:
    /// </summary>
    internal class EmptyTeam : AbstractTeam
    {
        public override AbstractGame Game { get; }
        public override PlayerTeam Enemy { get; }
        public Character DieCharacter { get; }
        public PlayerTeam Team { get; }
        /// <summary>
        /// 对于空队伍，很难说是不是真有用...
        /// </summary>
        public override int CurrCharacter { get; internal set; }
        //TODO:characters
        public override Character[] Characters { get; protected init; }

        public EmptyTeam(Character die, PlayerTeam team) : base(team.TeamIndex)
        {
            DieCharacter = die;
            Team = team;
            CurrCharacter = team.CurrCharacter;
            Enemy = team.Enemy;
            Game = team.Game;
        }
        public void DieTrigger(AbstractSender sender)
        {
            DieCharacter.GetPersistentHandlers(sender)?.Invoke(this, sender, null);
        }
        /// <summary>
        /// 直接调用原Team的EffectTrigger
        /// </summary>
        public override void EffectTrigger(AbstractSender sender, AbstractVariable? variable = null) => Team.EffectTrigger(sender, variable);
    }
}
