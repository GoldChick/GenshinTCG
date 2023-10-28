using TCGBase;

namespace Sample
{
    public class Sample_Register : AbstractRegister
    {
        public override void RegisterActionCard(IRegistryConsumer<AbstractCardAction> consumer)
        {
        }

        public override void RegisterCharacter(IRegistryConsumer<AbstractCardCharacter> consumer)
        {
        }

        public override void RegisterEffect(IRegistryConsumer<AbstractCardPersistentEffect> consumer)
        {
        }

        public override void RegisterSummon(IRegistryConsumer<AbstractCardPersistentSummon> consumer)
        {
        }

        public override void RegisterSupport(IRegistryConsumer<AbstractCardPersistentSupport> consumer)
        {
        }
    }
}
