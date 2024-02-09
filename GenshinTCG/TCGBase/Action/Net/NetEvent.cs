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
        public NetEvent(NetOperation operation)
        {
            Operation = operation;
            _costargs = new int[8];
            AdditionalTargetArgs = Array.Empty<int>();
        }
        public NetEvent(NetOperation operation, params int[]? cost)
        {
            Operation = operation;
            Normalize.CostNormalize(cost, out _costargs);
            AdditionalTargetArgs = Array.Empty<int>();
        }
        [JsonConstructor]
        public NetEvent(NetOperation operation, int[] costargs, int[] additionaltargetArgs)
        {
            Operation = operation;
            Normalize.CostNormalize(costargs, out _costargs);
            AdditionalTargetArgs = additionaltargetArgs ?? Array.Empty<int>();
        }
    }
}
