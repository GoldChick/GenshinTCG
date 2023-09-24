using System.Text.Json.Serialization;

namespace TCGBase
{
    public class NetEventRequire
    {
        public Cost Cost { get; init; }

        /// <param name="dices">any 冰水火雷岩草风</param>
        [JsonConstructor]
        public NetEventRequire(Cost? cost)
        {
            Cost = cost ?? new Cost(false, 0);
        }
        public NetEventRequire()
        {
            Cost = new Cost(false, 128);
        }
    }
}
