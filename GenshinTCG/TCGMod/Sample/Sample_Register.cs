using TCGCard;
using TCGRule;
using TCGUtil;

namespace Sample
{
    public class Sample_Register : AbstractRegister
    {
        public override void RegisterActionCard(IConsumer<ICardAction> consumer)
        {
        }

        public override void RegisterCharacter(IConsumer<ICardCharacter> consumer)
        {
        }

        public override void RegisterEffect(IConsumer<AbstractCardEffect> consumer)
        {
        }

        public override void RegisterSummon(IConsumer<AbstractCardSummon> consumer)
        {
        }

        public override void RegisterSupport(IConsumer<AbstractCardSupport> consumer)
        {
        }
    }
}
