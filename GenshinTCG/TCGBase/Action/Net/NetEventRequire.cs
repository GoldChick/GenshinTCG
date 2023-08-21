using System.Text.Json.Serialization;
using TCGCard;

namespace TCGBase
{
    public class NetEventRequire : ITargetSelector
    {
        public Cost Cost { get; init; }
        public TargetEnum[] TargetEnums { get; init; }

        /// <param name="dices">any 冰水火雷岩草风</param>
        [JsonConstructor]
        public NetEventRequire(Cost? cost, TargetEnum[]? targetEnums = null)
        {
            Cost = cost ?? new Cost(false, 0);
            TargetEnums = targetEnums ?? Array.Empty<TargetEnum>();
        }
        /// <param name="dices">any 冰水火雷岩草风</param>
        public NetEventRequire(Cost? cost, List<TargetEnum>? targetEnums = null)
        {
            Cost = cost ?? new Cost(false, 0);
            TargetEnums = (targetEnums ?? new()).ToArray();
        }
        public NetEventRequire()
        {
            Cost = new Cost(false, 128);
            TargetEnums = Array.Empty<TargetEnum>();
        }
    }
}
