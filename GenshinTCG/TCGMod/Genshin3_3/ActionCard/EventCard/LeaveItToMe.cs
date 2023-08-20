using TCGBase;
using TCGCard;
using TCGGame;

namespace Genshin3_3
{
    public class LeaveItToMe : ICardEvent
    {
        public LeaveItToMe()
        {
        }

        public int MaxNumPermitted => throw new NotImplementedException();

        public string NameID => "leave_it_to_me";

        public bool SameDice => throw new NotImplementedException();

        public bool CostSame => throw new NotImplementedException();

        public string[] Tags => throw new NotImplementedException();

        public int[] Costs => throw new NotImplementedException();

        public void AfterUseAction()
        {
            throw new NotImplementedException();
        }

        public bool CanBeArmed()
        {
            throw new NotImplementedException();
        }

        public bool CanBeUsed(AbstractGame game, int meIndex)
        {
            throw new NotImplementedException();
        }
    }
}
