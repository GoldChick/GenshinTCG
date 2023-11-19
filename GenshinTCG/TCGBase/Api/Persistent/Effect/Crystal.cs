using TCGBase;

namespace Minecraft
{
    public class Crystal : AbstractPersistentShieldYellow
    {
        public override int InitialUseTimes => 1;
        public override int MaxUseTimes => 2;
        public Crystal() : base()
        {
        }
    }
}
