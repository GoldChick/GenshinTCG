using TCGCard;
namespace TCGRule
{
    public abstract class AbstractRegister
    {
        public abstract void RegisterCharacter(Consumer<ICardCharacter> consumer);
        public abstract void RegisterActionCard(Consumer<ICardAction> consumer);

        public abstract void RegisterSupport(Consumer<ISupport> consumer);
        public abstract void RegisterEffect(Consumer<IEffect> consumer);
        public abstract void RegisterSummon(Consumer<ISummon> consumer);
    }
}
