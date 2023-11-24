namespace TCGBase
{
    /// <summary>
    /// 事件牌，比较笼统，如产生buff、战斗行动、操作召唤物等
    /// </summary>
    public abstract class AbstractCardEvent : AbstractCardAction, IDamageSource, ITargetSelector
    {
        public DamageSource DamageSource => DamageSource.NoWhere;

        public virtual TargetDemand[] TargetDemands => Array.Empty<TargetDemand>();
    }
}
