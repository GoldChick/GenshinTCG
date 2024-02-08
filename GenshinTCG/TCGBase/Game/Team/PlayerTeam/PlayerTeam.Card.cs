namespace TCGBase
{
    public partial class PlayerTeam
    {
        public int CardNum => CardsInHand.Count();
        /// <summary>
        /// 抽取num张<br/>
        /// 若指定type : 究极定向检索，检索type类型和type的子类
        /// </summary>
        public void RollCard(int num, Type? type = null)
        {
            var collection = type == null ? LeftCards : LeftCards.Where(c => c.GetType().IsAssignableTo(type));
            for (int i = 0; i < num; i++)
            {
                if (collection.Any())
                {
                    int index = Random.Next(collection.Count());
                    Game.BroadCast(ClientUpdateCreate.CardUpdate(TeamIndex, ClientUpdateCreate.CardUpdateCategory.Pop));
                    GainCard(collection.ElementAt(index));
                    LeftCards.RemoveAt(index);
                }
            }
        }
        public void GainCard(AbstractCardAction card) => CardsInHand.Add(card);
        public List<AbstractCardBase> GetCards() => CardsInHand.Select(p => p.CardBase).ToList();
        public int TryDestroyAllCard(Predicate<AbstractCardBase> condition) => CardsInHand.TryDestroyAll(condition);
    }
}
