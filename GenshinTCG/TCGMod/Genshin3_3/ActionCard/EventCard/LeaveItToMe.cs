using TCGBase;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class LeaveItToMe : AbstractCardEvent
    {
        public override string NameID => "leaveittome";

        public override bool CostSame => false;

        public override string[] SpecialTags => Array.Empty<string>();

        public override int[] Costs => Array.Empty<int>();


        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            me.AddPersistent(new LeaveItToMeEffect());
            Logger.Error("使用了交给我吧!");
        }
        public class LeaveItToMeEffect : AbstractCardPersistentEffect
        {
            public override int MaxUseTimes => 1;

            public override string NameID => "leaveittome_effect";

            public override PersistentTriggerDictionary TriggerDic => new() { 
                { SenderTag.AfterSwitch, (me,p,s,v)=>
                {
                    if (v is FastActionVariable fav)
                    {
                        if (!fav.Fast)
                        {
                            fav.Fast = true;
                            p.AvailableTimes--;
                        }
                    }
                }
                }
            };
        }
    }
}
