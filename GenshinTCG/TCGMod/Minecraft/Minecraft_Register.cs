using TCGBase;

namespace Minecraft
{
    public class Minecraft_Register : AbstractRegister
    {
        public override void RegisterTriggerable(IRegistryConsumer<AbstractTriggerable> consumer)
        {
            consumer.Accept(new HydroQ());
            consumer.Accept(new WangshuInn());
            consumer.Accept(new ColoredSummon(new(DamageElement.Anemo, 2)));
        }
    }
}
