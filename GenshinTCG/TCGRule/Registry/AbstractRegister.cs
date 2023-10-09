using TCGCard;
using TCGUtil;

namespace TCGRule
{
    public abstract class AbstractRegister
    {
        public abstract void RegisterCharacter(IConsumer<AbstractCardCharacter> consumer);
        public abstract void RegisterActionCard(IConsumer<AbstractCardAction> consumer);

        /// TODO:以下三个可能是不需要注册的(?)

        public abstract void RegisterSupport(IConsumer<AbstractCardSupport> consumer);
        public abstract void RegisterEffect(IConsumer<AbstractCardEffect> consumer);
        public abstract void RegisterSummon(IConsumer<AbstractCardSummon> consumer);
    }
}
