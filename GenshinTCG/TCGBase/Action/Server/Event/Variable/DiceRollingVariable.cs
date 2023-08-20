using System.Diagnostics.CodeAnalysis;

namespace TCGBase
{
    public class DiceRollingVariable : AbstractVariable
    {
        public override string VariableName => Tags.VariableTags.DICE_ROLLING;

        /// <summary>
        /// 投掷骰子方在Game.Teams中的index
        /// </summary>
        public int Owner { get; init; }
        /// <summary>
        /// 为正表示新扔的骰子，为负表示重投已有
        /// </summary>
        public int DiceNum { get; set; }
        public int RollingTimes { get; set; }
        /// <summary>
        /// 首次一定会投掷出的骰子，前DiceNum个有效
        /// </summary>
        public List<int> InitialDices { get; init; }

        public DiceRollingVariable(int owner, int rollingTimes, [NotNull] List<int> ints)
        {
            Owner = owner;
            DiceNum = -1;
            RollingTimes = rollingTimes;
            InitialDices = ints ?? new();
        }

        public DiceRollingVariable(int owner)
        {
            DiceNum = 8;
            RollingTimes = 1;
            InitialDices = new();
            Owner = owner;
        }
    }
}
