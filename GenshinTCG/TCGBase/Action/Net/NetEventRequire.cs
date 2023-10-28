using System.Text.Json.Serialization;

namespace TCGBase
{
    public class NetEventRequire
    {
        public DiceCost Cost { get; init; }

        /// <param name="dices">any 冰水火雷岩草风</param>
        [JsonConstructor]
        public NetEventRequire(DiceCost? cost)
        {
            Cost = cost ?? new DiceCost(false, 0);
        }
        public NetEventRequire()
        {
            Cost = new DiceCost(false, 128);
        }
    }
}
