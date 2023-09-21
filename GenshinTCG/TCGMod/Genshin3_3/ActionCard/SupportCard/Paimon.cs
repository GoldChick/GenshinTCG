using TCGCard;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class Paimon : ICardSupport
    {
        public int MaxNumPermitted => 2;

        public string NameID => "paimon";

        public string[] Tags => new string[] { TCGBase.Tags.CardTags.AssistTags.PARTNER };

        public int[] Costs => new int[] { 3 };

        public bool CostSame => true;

        public void AfterUseAction(AbstractGame game, int meIndex)
        {
            Logger.Print("打出了一张大pi!");
        }

        public bool CanBeUsed(AbstractGame game, int meIndex) => true;
    }
}
