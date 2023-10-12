using System;
using TCGBase;
using TCGCard;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class LeaveItToMe : AbstractCardEvent
    {
        public override string NameID => "leaveittome";

        public override bool CostSame => false;

        public override string[] Tags => Array.Empty<string>();

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

            public override Dictionary<string, IPersistentTrigger> TriggerDic => new() { { TCGBase.Tags.SenderTags.AFTER_SWITCH, new FastSwitchTrigger() } };

            private class FastSwitchTrigger : IPersistentTrigger
            {
                public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
                {
                    if (variable is FastActionVariable fav)
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
}
