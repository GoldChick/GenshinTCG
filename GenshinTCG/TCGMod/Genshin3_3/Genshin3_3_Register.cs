using TCGBase;

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
            consumer.Accept(new 参量质变仪());
            consumer.Accept(new 赌徒());

            consumer.Accept(new 寒天宣命祝词());

        }

        public override void RegisterCharacter(IRegistryConsumer<AbstractCardCharacter> consumer)
        {
            consumer.Accept(new Ayaka());
            consumer.Accept(new Qiqi());

            consumer.Accept(new Mona());
         
            consumer.Accept(new Yoimiya());
            consumer.Accept(new XiangLing());

            consumer.Accept(new Keqing());
            consumer.Accept(new YaeMiko());
            
            consumer.Accept(new 丘丘岩盔王());
            
            consumer.Accept(new Nahida());
            
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
