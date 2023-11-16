namespace TCGBase
{
    public abstract class AbstractRegister
    {
        public abstract void RegisterCharacter(IRegistryConsumer<AbstractCardCharacter> consumer);
        public abstract void RegisterActionCard(IRegistryConsumer<AbstractCardAction> consumer);
        /// <summary>
        /// 在此处注册只是为了便于给他一个namespace，用来提供文本<br/>
        /// </summary>
        public virtual void RegisterPersistent(IRegistryConsumer<AbstractCardPersistent> consumer) { }
    }
}
