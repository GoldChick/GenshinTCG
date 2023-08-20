using TCGBase;
using TCGCard;

namespace TCGGame
{
    public class ActionCard 
    {
        public ICardAction Card { get; protected set; }

        public ActionCard(ICardAction card)
        {
            Card = card;
        }
        public bool EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
            //TODO
            return true;
        }
    }
}
