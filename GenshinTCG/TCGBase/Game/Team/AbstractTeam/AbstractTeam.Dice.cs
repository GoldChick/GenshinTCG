namespace TCGBase
{
    public partial class AbstractTeam
    {
        public virtual int DiceNum { get => 0; }
        /// <summary>
        /// 杂色骰优先按照数量排列，再按照冰水火雷岩草风的顺序排列，
        /// </summary>
        /// <returns></returns>
        public virtual List<(int count, int element)> GetSortedDices() => new();
        /// <summary>
        /// 返回int[8],第i个成员表示第i种元素骰的数量(默认顺序:万能 冰水火雷岩草风)
        /// </summary>
        public virtual int[] GetDicesArray() => new int[8];
        /// <summary>
        /// int值代表元素类型
        /// </summary>
        public virtual void GainDice(params int[] dices)
        {
        }
        public virtual void TryRemoveDice(int element)
        {
        }
    }
}
