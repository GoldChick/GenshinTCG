using TCGCard;
using TCGRule;
using TCGUtil;

namespace Genshin3_3
{
    public class Genshin3_3_Register : AbstractRegister
    {
        public override void RegisterActionCard(IConsumer<ICardAction> consumer)
        {
            consumer.Accept(new SacrificialSword());

            consumer.Accept(new LeaveItToMe());

            consumer.Accept(new Paimon());

        }

        public override void RegisterCharacter(IConsumer<ICardCharacter> consumer)
        {
            consumer.Accept(new Keqing());
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
