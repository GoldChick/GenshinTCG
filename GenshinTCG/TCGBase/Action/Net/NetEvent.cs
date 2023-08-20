﻿using TCGUtil;

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
        /// Target对客户端的要求比较高,就不在服务端写逻辑了
        /// </summary>
        public int[]? TargetArgs { get;  set; }
        public NetEvent(NetAction action)
        {
            Action = action;
        }
    }
}
