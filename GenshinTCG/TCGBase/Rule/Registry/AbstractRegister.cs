namespace TCGBase
{
    public abstract class AbstractRegister
    {
        public abstract void RegisterActionCard(IRegistryConsumer<AbstractCardAction> consumer);
        public abstract void RegisterEffectCard(IRegistryConsumer<AbstractCardEffect> consumer);
        public abstract void RegisterTriggerable(IRegistryConsumer<AbstractTriggerable> consumer);
    }
}
