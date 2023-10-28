namespace TCGBase
{
    public abstract class AbstractRegister
    {
        public abstract void RegisterCharacter(IRegistryConsumer<AbstractCardCharacter> consumer);
        public abstract void RegisterActionCard(IRegistryConsumer<AbstractCardAction> consumer);

        /// TODO:以下三个可能是不需要注册的(?)

        public virtual void RegisterSupport(IRegistryConsumer<AbstractCardPersistentSupport> consumer) { }
        public virtual void RegisterEffect(IRegistryConsumer<AbstractCardPersistentEffect> consumer) { }
        public virtual void RegisterSummon(IRegistryConsumer<AbstractCardPersistentSummon> consumer) { }
    }
}
