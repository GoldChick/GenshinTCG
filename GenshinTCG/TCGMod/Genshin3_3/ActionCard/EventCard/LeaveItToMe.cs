using TCGCard;

namespace Genshin3_3
{
    public class LeaveItToMe : ICardEvent
    {
        public int MaxNumPermitted => throw new NotImplementedException();

        public string NameID => "leave_it_to_me";

        public HashSet<string> Tags => throw new NotImplementedException();

        public bool SameDice => throw new NotImplementedException();

        public Dictionary<string, int> Costs => throw new NotImplementedException();

        public void AfterUseAction()
        {
            throw new NotImplementedException();
        }

        public bool CanBeArmed()
        {
            throw new NotImplementedException();
        }

        public bool CanBeUsed()
        {
            throw new NotImplementedException();
        }
    }
}
