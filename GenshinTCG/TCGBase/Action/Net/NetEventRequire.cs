namespace TCGBase
{
    public class NetEventRequire
    {
        public DiceCost Cost { get; init; }

        /// <param name="dices">any 冰水火雷岩草风</param>
        public NetEventRequire(DiceCost? cost)
        {
            Cost = cost ?? new DiceCost(false, 0);
        }
    }
}
