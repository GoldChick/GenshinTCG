namespace TCGBase
{
    public abstract class AbstractRegister
    {
        public abstract void RegisterCharacter(IRegistryConsumer<AbstractCardCharacter> consumer);
        public abstract void RegisterActionCard(IRegistryConsumer<AbstractCardAction> consumer);
        public abstract void RegisterTriggerable(IRegistryConsumer<AbstractCustomTriggerable> consumer);
    }
}
