using TCGBase;

namespace Sample
{
    public class Sample_Register : AbstractRegister
    {
        public override void RegisterActionCard(IRegistryConsumer<AbstractCardAction> consumer)
        {
            throw new NotImplementedException();
        }

        public override void RegisterCharacter(IRegistryConsumer<AbstractCardCharacter> consumer)
        {
            throw new NotImplementedException();
        }

        public override void RegisterTriggerable(IRegistryConsumer<AbstractCustomTriggerable> consumer)
        {
            throw new NotImplementedException();
        }
    }
}
