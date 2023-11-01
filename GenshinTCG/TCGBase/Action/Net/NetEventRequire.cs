namespace TCGBase
{
    public class NetEventRequire
    {
        public DiceCostVariable Cost { get; init; }

        /// <param name="dices">any 冰水火雷岩草风</param>
        public NetEventRequire(DiceCostVariable? cost)
        {
            Cost = cost ?? new DiceCostVariable(false, 0);
        }
    }
}
