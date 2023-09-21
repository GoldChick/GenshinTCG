using System.Text.Json;
using TCGBase;
using TCGCard;
using TCGUtil;

namespace TCGGame
{
    public class ActionCard:IPrintable
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

        public void Print()
        {
            Logger.Print($"{Card.NameID} {JsonSerializer.Serialize(Card.Tags)}");
        }
    }
}
