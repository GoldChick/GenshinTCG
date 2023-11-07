namespace TCGBase
{
    public partial class PlayerTeam
    {
        /// <summary>
        /// 究极定向检索
        /// </summary>
        public void RollCard(Type type)
        {
            var col = LeftCards.Where(c => c.Card.GetType() == type);
            if (col.Any())
            {
                var c = col.ElementAt(Random.Next(col.Count()));
                LeftCards.Remove(c);
                Game.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Pop));
                GainCard(c);
            }
        }
        public void RollCard(int num)
        {
            for (int i = 0; i < num; i++)
            {
                if (LeftCards.Count > 0)
                {
                    var c = LeftCards[Random.Next(LeftCards.Count)];
                    LeftCards.Remove(c);
                    Game.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Pop));
                    GainCard(c);
                }
            }
        }
        internal void GainCard(ActionCard card)
        {
            if (CardsInHand.Count >= 10)
            {
                Game.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Broke, card.Card.NameID));
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
