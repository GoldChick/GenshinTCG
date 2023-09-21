using TCGUtil;

namespace TCGBase
{
    /// <summary>
    /// 用于客户端和服务端之间信息传输的Event
    /// 首先产生其中的Action，然后根据情况补充Costs&Target
    /// 客户端可以任意指定数据,由服务端判定是否合理(防作弊)
    /// </summary>
    public class NetEvent
    {
        private int[]? _costArgs;
        public NetAction Action { get; private init; }
        public int[]? CostArgs { get => _costArgs; set => Normalize.CostNormalize(value, out _costArgs); }
        /// <summary>
        /// 并不在NetAction.Index中，而是额外一些的Target，如[送你一程]=>[Summon_Enemy]
        /// </summary>
        public int[]? AdditionalTargetArgs { get;  set; }
        public NetEvent(NetAction action)
        {
            Action = action;
        }
        //TODO:4test
        public NetEvent(NetAction action,params int[]? cost)
        {
            Action = action;
            _costArgs = cost;
        }
    }
}
