using TCGBase;
using TCGCard;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class LeaveItToMe : ICardEvent
    {
        public int MaxNumPermitted => 2;

        public string NameID => "leaveittome";

        public bool CostSame => false;

        public string[] Tags => Array.Empty<string>();

        public int[] Costs => Array.Empty<int>();

        public void AfterUseAction(AbstractGame game, int meIndex)
        {
            game.Teams[meIndex].AddPersistent(new LeaveItToMeEffect());
            Logger.Error("使用了交给我吧!");
        }

        public bool CanBeArmed() => true;

        public bool CanBeUsed(AbstractGame game, int meIndex) => true;
    }
}
