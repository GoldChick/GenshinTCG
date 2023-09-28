using TCGCard;
using TCGMod;
using TCGRule;
using TCGUtil;

namespace Genshin3_3
{
    public class Sample_Register : AbstractRegister
    {
        public override void RegisterActionCard(IConsumer<ICardAction> consumer)
        {
            consumer.Accept(new SacrificialSword());

            consumer.Accept(new LeaveItToMe());
            consumer.Accept(new XingTianZhiZhao());

            consumer.Accept(new Paimon());
            consumer.Accept(new 赌徒());

        }

        public override void RegisterCharacter(IConsumer<ICardCharacter> consumer)
        {
            consumer.Accept(new Keqing());
            consumer.Accept(new Mona());
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
