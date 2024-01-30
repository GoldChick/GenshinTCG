namespace TCGBase
{
    public partial class PlayerTeam
    {
        public override int CardNum => CardsInHand.Count;
        /// <summary>
        /// 抽取num张<br/>
        /// 若指定type : 究极定向检索，检索type类型和type的子类
        /// </summary>
        public override void RollCard(int num, Type? type = null)
        {
            var collection = type == null ? LeftCards : LeftCards.Where(c => c.GetType().IsAssignableTo(type));
            for (int i = 0; i < num; i++)
            {
                if (collection.Any())
                {
                    int index = Random.Next(collection.Count());
                    RealGame.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Pop));
                    GainCard(collection.ElementAt(index));
                    LeftCards.RemoveAt(index);
                }
            }
        }
        public void GainCard(AbstractCardAction card)
        {
            if (CardsInHand.Count >= 10)
            {
                RealGame.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Broke, card.Namespace, card.NameID));
            }
            else
            {
                CardsInHand.Add(card);
                RealGame.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Obtain, card.Namespace, card.NameID));
            }
        }
        public List<AbstractCardAction> GetCards() => CardsInHand.ToList();
        /// <summary>
        /// remove at index(0 to length-1)
        /// </summary>
        public void TryRemoveCard(int index)
        {
            if (index >= 0 && index < CardsInHand.Count)
            {
                CardsInHand.RemoveAt(index);
                RealGame.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Blend, index));
            }
        }
        /// <summary>
        /// remove first one
        /// </summary>
        public void TryRemoveCard(string cardNamespace, string cardNameID) => TryRemoveCard(CardsInHand.FindIndex(p => p.Namespace == cardNamespace && p.NameID == cardNameID));
        public void TryRemoveAllCard(Func<AbstractCardAction, bool> predicate)
        {
            for (int i = CardsInHand.Count - 1; i >= 0; i--)
            {
                if (predicate.Invoke(CardsInHand[i]))
                {
                    CardsInHand.RemoveAt(i);
                    RealGame.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Blend, i));
                }
            }
        }
    }
}
