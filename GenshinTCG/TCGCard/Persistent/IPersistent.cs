namespace TCGCard
{

    /// <summary>
    /// TODO:
    /// IEffect\ISupport @deprecated
    /// 服务端不需要
    /// </summary>

    public interface IPersistent : ICardServer
    {
        /// <summary>
        /// 可用次数为0时是否立即删除<br/>
        /// 为false时，需要自己手动控制AbstractPersistent.Active，每次结算(update())会清除所有deactive的effect
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
