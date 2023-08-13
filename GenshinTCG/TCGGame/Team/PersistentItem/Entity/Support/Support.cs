using TCGBase;
using TCGCard;

namespace TCGGame
{
    public class Support : AbstractPersistent<ISupport>
    {
        public override void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
        }
    }
}
