namespace TCGBase
{
    internal class DiceRollingVariable : AbstractVariable
    {
        public override string VariableName => Tags.VariableTags.DICE_ROLLING;

        public int DiceNum { get; set; }
        public int RollingTimes { get; set; }
        /// <summary>
        /// 首次一定会投掷出的骰子，前DiceNum个有效
        /// </summary>
        public List<int> InitialDices { get; init; }


        public DiceRollingVariable()
        {
            DiceNum = 8;
            RollingTimes = 1;
            InitialDices = new();
        }
    }
}
