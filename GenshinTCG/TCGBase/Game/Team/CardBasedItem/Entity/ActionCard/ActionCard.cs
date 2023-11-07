namespace TCGBase
{
    public class ActionCard
    {
        public AbstractCardAction Card { get; }

        public ActionCard(AbstractCardAction card)
        {
            Card = card;
        }
    }
}
