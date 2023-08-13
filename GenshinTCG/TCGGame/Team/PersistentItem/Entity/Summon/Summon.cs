using TCGBase;
using TCGCard;

namespace TCGGame
{
    public class Summon : AbstractPersistent<ISummon>
    {
        public override void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
        }
    }
}
