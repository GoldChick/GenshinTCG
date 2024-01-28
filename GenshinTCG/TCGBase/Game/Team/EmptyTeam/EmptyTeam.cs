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
        /// <summary>
        /// 对于空队伍，很难说是不是真有用...
        /// </summary>
        public override int CurrCharacter { get; internal set; }


        public EmptyTeam(Character die, PlayerTeam team) : base(team.TeamIndex)
        {
            DieCharacter = die;
            CurrCharacter = team.CurrCharacter;
            Enemy = team.Enemy;
            Game = team.Game;
        }
        public override void EffectTrigger(AbstractSender sender, AbstractVariable? variable = null)
        {
            var handlers = DieCharacter.Effects.GetPersistentHandlers(new DieSender(TeamIndex, DieCharacter.PersistentRegion));
            handlers?.Invoke(this, sender, variable);
        }
    }
}
