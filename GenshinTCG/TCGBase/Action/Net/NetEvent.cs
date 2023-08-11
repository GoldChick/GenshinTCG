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
        public AbstractAction Action { get; private init; }
        public int[]? CostArgs { get => _costArgs; }
        /// <summary>
        /// Target对客户端的要求比较高,就不在服务端写逻辑了
        /// </summary>
        public int[]? TargetArgs { get; private set; }
        public NetEvent(AbstractAction action)
        {
            Action = action;
        }
        public void AddCost(params int[] cost) => TCGUtil.Cost.NormalizeCost(cost, out _costArgs);
        public void AddTarget(params int[] target)=>TargetArgs=target;
    }
}
