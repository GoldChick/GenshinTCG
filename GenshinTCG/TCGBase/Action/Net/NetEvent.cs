using System.Text.Json.Serialization;

namespace TCGBase
{
    /// <summary>
    /// 用于客户端和服务端之间信息传输的Event
    /// </summary>
    public class NetEvent
    {
        private readonly int[] _costargs;
        public NetOperation Operation { get; private init; }
        public int[] CostArgs { get=>_costargs; }
        /// <summary>
        /// 并不在NetAction.Index中，而是额外一些的Target，如[送你一程]=>[Summon_Enemy]
        /// </summary>
        public int[] AdditionalTargetArgs { get; private set; }
        public NetEvent(NetOperation action)
        {
            Operation = action;
            _costargs = new int[8];
            AdditionalTargetArgs = Array.Empty<int>();
        }
        public NetEvent(NetOperation action, params int[]? cost)
        {
            Operation = action;
            Normalize.CostNormalize(cost, out _costargs);
            AdditionalTargetArgs = Array.Empty<int>();
        }
        [JsonConstructor]
        public NetEvent(NetOperation action, int[] costargs, int[] additionaltargetArgs)
        {
            Operation = action;
            Normalize.CostNormalize(costargs, out _costargs);
            AdditionalTargetArgs = additionaltargetArgs ?? Array.Empty<int>();
        }
    }
}
