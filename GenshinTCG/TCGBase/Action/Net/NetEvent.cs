namespace TCGBase
{
    /// <summary>
    /// 用于客户端和服务端之间信息传输的Event
    /// </summary>
    public class NetEvent
    {
        private readonly int[] _costargs;
        public NetAction Action { get; private init; }
        public int[] CostArgs { get=>_costargs; }
        /// <summary>
        /// 并不在NetAction.Index中，而是额外一些的Target，如[送你一程]=>[Summon_Enemy]
        /// </summary>
        public int[] AdditionalTargetArgs { get; private set; }
        public NetEvent(NetAction action)
        {
            Action = action;
            _costargs = new int[8];
            AdditionalTargetArgs = Array.Empty<int>();
        }
        public NetEvent(NetAction action, params int[]? cost)
        {
            Action = action;
            Normalize.CostNormalize(cost, out _costargs);
            AdditionalTargetArgs = Array.Empty<int>();
        }
        public NetEvent(NetAction action, int[]? cost, int[]? targetArgs)
        {
            Action = action;
            Normalize.CostNormalize(cost, out _costargs);
            AdditionalTargetArgs = targetArgs ?? Array.Empty<int>();
        }
    }
}
