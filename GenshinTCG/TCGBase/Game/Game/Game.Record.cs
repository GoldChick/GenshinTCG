namespace TCGBase
{
    public partial class Game
    {
        /// <summary>
        /// int: teamid<br/>
        /// bool: 是否快速行动（很重要，通过回放跳转仅会在不同战斗行动之间跳转）
        /// </summary>
        public List<List<(int,bool, NetEvent)>> Records { get; }
    }
}
