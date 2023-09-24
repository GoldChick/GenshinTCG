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

        public class LeaveItToMeEffect : IEffect
        {
            public bool Stackable => false;

            public bool DeleteWhenUsedUp => true;

            public int MaxUseTimes => 1;

            public bool Visible => true;

            public string NameID => "leaveittome_effect";

            public string[] Tags => new string[] { };

            public void EffectTrigger(AbstractTeam me, AbstractTeam enemy, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
            {
                if (sender is SwitchSender ss && variable is FastActionVariable fav)
                {
                    Logger.Error("SwitchSender 用 FastActionVariable 触发了 交给我吧！");
                    if (!fav.Fast)
                    {
                        fav.Fast = true;
                        persitent.AvailableTimes--;
                    }
                }
            }
        }
    }
}
