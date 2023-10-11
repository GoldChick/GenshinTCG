using TCGCard;
using TCGMod;
using TCGRule;
using TCGUtil;

namespace Genshin3_3
{
    public class Sample_Register : AbstractRegister
    {
        public override void RegisterActionCard(IRegistryConsumer<AbstractCardAction> consumer)
        {
            consumer.Accept(new SacrificialSword());

            consumer.Accept(new LeaveItToMe());
            consumer.Accept(new XingTianZhiZhao());

            consumer.Accept(new Paimon());
            consumer.Accept(new 赌徒());

        }

        public override void RegisterCharacter(IRegistryConsumer<AbstractCardCharacter> consumer)
        {
            consumer.Accept(new Keqing());
            consumer.Accept(new Mona());
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
