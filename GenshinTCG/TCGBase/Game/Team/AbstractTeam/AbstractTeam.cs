namespace TCGBase
{
    public abstract partial class AbstractTeam
    {
        public abstract AbstractGame Game { get; }
        public abstract PlayerTeam Enemy { get; }
        /// <summary>
        /// 在Game.Teams中的index
        /// </summary>
        public int TeamIndex { get; private init; }
        /// <summary>
        /// 只允许使用队内的random
        /// </summary>
        public CounterRandom Random { get; init; }
        public TeamSpecialState SpecialState { get; init; }
        public abstract int CurrCharacter { get; internal set; }
        protected private AbstractTeam(int teamindex)
        {
            TeamIndex = teamindex;
            Random = new();//TODO:SEED
            SpecialState = new();
        }
    }
}
