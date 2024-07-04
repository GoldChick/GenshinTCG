using System.Linq;

namespace TCGBase
{
    public partial class PlayerTeam
    {
        public int CardNum => CardsInHand.Count();
        public void RollCard(int num, IEnumerable<AbstractCardAction> pool)
        {
            var collection = pool;
            for (int i = 0; i < num; i++)
            {
                if (collection.Any())
                {
                    int index = Random.Next(collection.Count());
                    Game.BroadCast(ClientUpdateCreate.CardUpdate(TeamID, ClientUpdateCreate.CardUpdateCategory.Pop));
                    GainCard(collection.ElementAt(index));
                    LeftCards.RemoveAt(index);
                }
            }
        }
        /// <summary>
        /// 抽取num张带有指定tag的牌
        /// </summary>
        public void RollCard(int num, Predicate<AbstractCardAction>? condition = null) => RollCard(num, condition == null ? LeftCards : LeftCards.Where(condition.Invoke));
        public void GainCard(AbstractCardAction card) => CardsInHand.Add(card);
        public List<AbstractCardBase> GetCards() => CardsInHand.Select(p => p.CardBase).ToList();
        public int TryDestroyAllCard(Predicate<AbstractCardBase> condition) => CardsInHand.TryDestroyAll(condition);
    }
}
