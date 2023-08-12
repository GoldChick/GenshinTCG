using TCGCard;
using TCGUtil;

namespace TCGRule
{
    public abstract class AbstractRegister
    {
        public abstract void RegisterCharacter(IConsumer<ICardCharacter> consumer);
        public abstract void RegisterActionCard(IConsumer<ICardAction> consumer);

        public abstract void RegisterSupport(IConsumer<ISupport> consumer);
        public abstract void RegisterEffect(IConsumer<IEffect> consumer);
        public abstract void RegisterSummon(IConsumer<ISummon> consumer);
    }
}
