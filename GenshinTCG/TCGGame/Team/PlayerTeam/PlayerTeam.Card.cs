using TCGCard;
using TCGUtil;

namespace TCGGame
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
                    GainCard(c);
                }
            }
        }
        /// <param name="num">最多允许更换牌的数量，小于等于0代表无限</param>
        public void ReRollCard(int num=-1)
        {
            Logger.Error("ReRollCard还没做！");
        }
        public void GainCard(ActionCard card)
        {
            if (CardsInHand.Count > 10)
            {
                Logger.Error($"爆牌了！爆了{card.Card.NameID}");
            }else
            {
                CardsInHand.Add(card);
                //TODO: sort?
            }
        }
        public void GainCard(AbstractCardAction card)=>GainCard(new ActionCard(card));
    }
}
