using TCGGame;

namespace TCGCard
{
    /// <summary>
    /// 支援牌，打出后在支援区生成某种东西
    /// </summary>
    public abstract class AbstractCardSupport : AbstractCardAction, ISinglePersistentProvider<AbstractCardPersistentSupport>
    {
        public abstract AbstractCardPersistentSupport PersistentPool { get; }
        /// <summary>
        /// default do nothing for Support Card
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
        }
    }
}
