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
        public List<ActionCard> GetCards() => CardsInHand.ToList();
        /// <summary>
        /// remove at index(0 to length-1)
        /// </summary>
        public void TryRemoveCard(int index)
        {
            if (index >= 0 && index < CardsInHand.Count)
            {
                CardsInHand.RemoveAt(index);
                Game.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Blend, index));
            }
        }
        /// <summary>
        /// remove first one
        /// </summary>
        public void TryRemoveCard(string cardNamespace, string cardNameID) => TryRemoveCard(CardsInHand.FindIndex(p => p.Card.Namespace == cardNamespace && p.Card.NameID == cardNameID));
        public void TryRemoveAllCard(Func<AbstractCardAction, bool> predicate)
        {
            for (int i = CardsInHand.Count - 1; i >= 0; i--)
            {
                if (predicate.Invoke(CardsInHand[i].Card))
                {
                    CardsInHand.RemoveAt(i);
                    Game.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Blend, i));
                }
            }
        }
    }
}
