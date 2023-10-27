using TCGBase;
using TCGUtil;

namespace TCGGame
{
    public class ActionCard:IPrintable
    {
        public AbstractCardAction Card { get; protected set; }

        public ActionCard(AbstractCardAction card)
        {
            Card = card;
        }
        public bool EffectTrigger(Game game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
            //TODO
            return true;
        }

        public void Print()
        {
            Logger.Print($"{Card.NameID}");
        }
    }
}
