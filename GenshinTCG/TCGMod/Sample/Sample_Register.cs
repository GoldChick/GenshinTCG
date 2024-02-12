using TCGBase;

namespace Sample
{
    public class Sample_Register : AbstractRegister
    {
        public override void RegisterActionCard(IRegistryConsumer<AbstractCardAction> consumer)
        {
            throw new NotImplementedException();
        }
        public override void RegisterEffectCard(IRegistryConsumer<AbstractCardEffect> consumer)
        {
            throw new NotImplementedException();
        }
        public override void RegisterTriggerable(IRegistryConsumer<AbstractTriggerable> consumer)
        {
            throw new NotImplementedException();
        }
    }
}
