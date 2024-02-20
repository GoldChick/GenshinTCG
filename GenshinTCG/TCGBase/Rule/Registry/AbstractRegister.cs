namespace TCGBase
{
    public abstract class AbstractRegister
    {
        public abstract void RegisterTriggerable(IRegistryConsumer<AbstractTriggerable> consumer);
    }
}
