namespace TCGBase
{
    public abstract class AbstractCardPersistentSummon : AbstractCardPersistent, ISinglePersistentProvider<AbstractCardPersistentSummon>, IDamageSource
    {
        public AbstractCardPersistentSummon PersistentPool => this;

        public DamageSource DamageSource => DamageSource.Summon;

        /// <summary>
        /// 重复刷新召唤物的时候会如何行动，默认为取[当前次数]和[最大次数]的最大值
        /// </summary>
        public virtual void Update(Persistent<AbstractCardPersistentSummon> persistent) => persistent.AvailableTimes = int.Max(persistent.AvailableTimes, MaxUseTimes);
    }
}
