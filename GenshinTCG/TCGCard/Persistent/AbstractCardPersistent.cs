using System.Diagnostics;

namespace TCGCard
{

    /// <summary>
    /// TODO:
    /// IEffect\ISupport @deprecated
    /// 服务端不需要
    /// </summary>

    public abstract class AbstractCardPersistent : AbstractCardServer
    {
        public abstract int MaxUseTimes { get; }
        /// <summary>
        /// string: SenderTag names<br/>
        /// team: team me<br/>
        /// persistent: this buff<br/>
        /// 在#TCGMod.Util#中提供一些预设，如刷新次数，清除，黄盾，紫盾等
        /// </summary>
        public abstract Dictionary<string, PersistentTrigger> TriggerDic { get; }
        /// <summary>
        /// 可用次数为0时是否立即删除(表现为记active为false，下次/本次结算完毕后清除)<br/>
        /// 为false时，需要自己手动控制AbstractPersistent.Active，每次结算(update())会清除所有deactive的effect
        /// </summary>
        public virtual bool DeleteWhenUsedUp { get => true; }
        /// <summary>
        /// 用来给客户端提供使用的表现参数<br/>
        /// 默认为可用次数，TODO:将来可能会删除？
        /// </summary>
        public virtual int[] Info(TCGGame.AbstractPersistent p)
        {
            Debug.Assert(p.NameID.Contains(NameID),$"AbstractCardPersistent: unknown behavior? {p.NameID} and {NameID}");
            return new int[] { p.AvailableTimes };
        }
    }
}
