namespace TCGCard
{

    /// <summary>
    /// TODO:
    /// IEffect\ISummon\ISupport @deprecated
    /// 服务端不需要
    /// </summary>

    public interface IPersistent : ICardServer
    {
        /// <summary>
        /// 可用次数为0时是否立即删除
        /// </summary>
        public bool DeleteWhenUsedUp { get; }
        public int MaxUseTimes { get; }
        /// <summary>
        /// string: SenderTag names<br/>
        /// team: team me<br/>
        /// persistent: this buff<br/>
        /// 在#TCGMod.Util#中提供一些预设，如刷新次数，清除，黄盾，紫盾等
        /// </summary>
        public Dictionary<string, IPersistentTrigger> TriggerDic { get; }

    }
}
