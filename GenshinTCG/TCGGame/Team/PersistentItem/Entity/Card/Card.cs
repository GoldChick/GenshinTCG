using TCGBase;
using TCGCard;

namespace TCGGame
{
    public class Card : AbstractPersistent<ICardAction>
    {
        public Card(ICardAction card)
        {
            Card = card;
        }
        public override void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
        }
    }
}
