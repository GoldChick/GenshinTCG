using System.Diagnostics.CodeAnalysis;

namespace TCGBase
{
    public class DiceRollingVariable : AbstractVariable
    {

        /// <summary>
        /// 为正表示新扔的骰子，为负表示重投已有
        /// </summary>
        public int DiceNum { get; set; }
        /// <summary>
        /// 剩余重投次数
        /// </summary>
        public int RollingTimes { get; set; }
        /// <summary>
        /// 首次一定会投掷出的骰子，前DiceNum个有效
        /// </summary>
        public List<int> InitialDices { get; init; }

        public DiceRollingVariable(int owner, int rollingTimes, [NotNull] List<int> ints)
        {
            DiceNum = -1;
            RollingTimes = rollingTimes;
            InitialDices = ints ?? new();
        }

        public DiceRollingVariable()
        {
            DiceNum = 8;
            RollingTimes = 1;
            InitialDices = new();
        }
        /// <summary>
        /// 仅为单次重投设计，无rollingtimes<br/>
        /// 并且presets中的骰子不会占用dicenum中的数量
        /// </summary>
        /// <param name="dicenum"></param>
        internal DiceRollingVariable(int dicenum, List<int>? presets = null)
        {
            DiceNum = dicenum;
            InitialDices = presets ?? new();
        }
    }
}
