namespace TCGBase
{
    public enum AssistTags
    {
        None,
        Place,
        Partner,
        Item,
    }
    /// <summary>
    /// 支援牌，打出后在支援区生成某种东西
    /// </summary>
    public abstract class AbstractCardSupport : AbstractCardAction
    {
        public virtual AssistTags AssistTag { get => AssistTags.None; }
        /// <summary>
        /// default do nothing for Support Card<br/>
        /// 或许可以用来加个入场效果
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null) { }

        /// <summary>
        /// 产生时候的基础使用次数，默认和[最大次数]一样
        /// </summary>
        public virtual int InitialUseTimes { get => MaxUseTimes; }
        public abstract int MaxUseTimes { get; }
        /// <summary>
        /// team: team me<br/>
        /// persistent: this support<br/>
        /// </summary>
        public abstract PersistentTriggerDictionary TriggerDic { get; }
        /// <summary>
        /// 可用次数为0时是否立即删除(表现为记active为false，下次/本次结算完毕后清除)<br/>
        /// 为false时，需要自己手动控制AbstractPersistent.Active，每次结算(update())会清除所有deactive的effect
        /// </summary>
        public virtual bool DeleteWhenUsedUp { get => true; }
        /// <summary>
        /// 用来给客户端提供使用的表现参数
        /// </summary>
        public virtual int[] Info(AbstractPersistent p) => new int[] { p.AvailableTimes };
    }
}
