namespace TCGBase
{
    public abstract class AbstractCardPersistentSummon : AbstractCardPersistent, ISinglePersistentProvider<AbstractCardPersistentSummon>, IDamageSource
    {
        public AbstractCardPersistentSummon PersistentPool => this;

        public DamageSource DamageSource => DamageSource.Summon;

       }
}
