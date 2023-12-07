using TCGBase;

namespace Minecraft
{
    public class Summon_Burning : AbstractSimpleSummon
    {
        public override int InitialUseTimes => 1;
        public Summon_Burning() : base(3, 1, 2)
        {
        }
    }
}
