using TCGCard;
using TCGUtil;

namespace TCGRule
{
    public abstract class AbstractRegister
    {
        public abstract void RegisterCharacter(IConsumer<ICardCharacter> consumer);
        public abstract void RegisterActionCard(IConsumer<ICardAction> consumer);

        /// TODO:以下三个可能是不需要注册的(?)

        public abstract void RegisterSupport(IConsumer<ISupport> consumer);
        public abstract void RegisterEffect(IConsumer<IEffect> consumer);
        public abstract void RegisterSummon(IConsumer<ISummon> consumer);
    }
}
