using TCGCard;
using TCGUtil;

namespace TCGRule
{
    public abstract class AbstractRegister
    {
        public abstract void RegisterCharacter(IRegistryConsumer<AbstractCardCharacter> consumer);
        public abstract void RegisterActionCard(IRegistryConsumer<AbstractCardAction> consumer);

        /// TODO:以下三个可能是不需要注册的(?)

        public virtual void RegisterSupport(IRegistryConsumer<AbstractCardSupport> consumer) { }
        public virtual void RegisterEffect(IRegistryConsumer<AbstractCardEffect> consumer) { }
        public virtual void RegisterSummon(IRegistryConsumer<AbstractCardSummon> consumer) { }
    }
}
