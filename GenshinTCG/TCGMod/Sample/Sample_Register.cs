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

        public override void RegisterEffect(IConsumer<IEffect> consumer)
        {
        }

        public override void RegisterSummon(IConsumer<ISummon> consumer)
        {
        }

        public override void RegisterSupport(IConsumer<ISupport> consumer)
        {
        }
    }
}
