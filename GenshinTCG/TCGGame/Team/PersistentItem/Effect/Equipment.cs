using TCGBase;
using TCGCard;

namespace TCGGame
{
    public class Equipment : AbstractPersistent<ICardEquipment>
    {
        public override void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
        }
    }
}
