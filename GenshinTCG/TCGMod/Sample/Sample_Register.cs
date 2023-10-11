using TCGCard;
using TCGRule;
using TCGUtil;

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

        public override void RegisterEffect(IRegistryConsumer<AbstractCardEffect> consumer)
        {
        }

        public override void RegisterSummon(IRegistryConsumer<AbstractCardSummon> consumer)
        {
        }

        public override void RegisterSupport(IRegistryConsumer<AbstractCardSupport> consumer)
        {
        }
    }
}
