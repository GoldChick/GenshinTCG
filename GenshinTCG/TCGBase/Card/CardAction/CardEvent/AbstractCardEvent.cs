namespace TCGBase
{
    /// <summary>
    /// 事件牌，比较笼统，如产生buff、战斗行动、操作召唤物等<br/>
    /// <b>事件牌本身并不能直接转移到场上</b>
    /// </summary>
    public abstract class AbstractCardEvent : AbstractCardAction, IDamageSource, ITargetSelector
    {
        public override DamageSource DamageSource => DamageSource.NoWhere;
        public virtual TargetDemand[] TargetDemands => Array.Empty<TargetDemand>();
        public override PersistentTriggerDictionary TriggerDic => new();
    }
}
