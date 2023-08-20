using TCGBase;
using TCGCard;
using TCGGame;

namespace Genshin3_3
{
    public class Paimon : ICardSupport
    {
        public int MaxNumPermitted => 2;

        public string NameID => "paimon";

        string[] ICardBase.Tags => throw new NotImplementedException();

        int[] ICost.Costs => throw new NotImplementedException();

        bool ICost.CostSame => throw new NotImplementedException();

        void IUsable.AfterUseAction()
        {
            throw new NotImplementedException();
        }

        bool IUsable.CanBeUsed(AbstractGame game, int meIndex)
        {
            throw new NotImplementedException();
        }
    }
}
