using TCGBase;
using TCGBase;
using TCGUtil;

namespace TCGBase
{
    public partial class PlayerTeam
    {
        public void RollCard(int num)
        {
            for (int i = 0; i < num; i++)
            {
                if (LeftCards.Count > 0)
                {
                    var c = LeftCards[Random.Next(LeftCards.Count)];
                    LeftCards.Remove(c);
                    Game.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Pop, c.Card.NameID));
                    GainCard(c);
                }
            }
        }
        public void GainCard(ActionCard card)
        {
            if (CardsInHand.Count > 10)
            {
                Game.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Consume, card.Card.NameID));
            }
            else
            {
                CardsInHand.Add(card);
                Game.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Obtain, card.Card.NameID));
            }
        }
        public void GainCard(AbstractCardAction card) => GainCard(new ActionCard(card));
    }
}
