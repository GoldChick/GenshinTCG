using TCGBase;

namespace Minecraft
{
    public class Effect_Crystal : AbstractPersistentShieldYellow
    {
        public override int InitialUseTimes => 1;
        public override int MaxUseTimes => 2;
        public Effect_Crystal() : base()
        {
        }
    }
}
