using TCGGame;

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
    public abstract class AbstractCardSupport : AbstractCardAction, ISinglePersistentProvider<AbstractCardPersistentSupport>
    {
        public virtual AssistTags AssistTag { get => AssistTags.None; }
        public abstract AbstractCardPersistentSupport PersistentPool { get; }
        /// <summary>
        /// default do nothing for Support Card<br/>
        /// 或许可以用来加个入场效果
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null) { }
    }
}
