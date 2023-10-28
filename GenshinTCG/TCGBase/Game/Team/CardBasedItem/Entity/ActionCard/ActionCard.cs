using TCGBase;

namespace TCGBase
{
    public class ActionCard
    {
        public AbstractCardAction Card { get; protected set; }

        public ActionCard(AbstractCardAction card)
        {
            Card = card;
        }
    }
}
