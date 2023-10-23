using System;
using TCGBase;
using TCGCard;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class Paimon : AbstractCardSupport
    {
        public override string NameID => "paimon";

        public override string[] SpecialTags => new string[] { TCGBase.Tags.CardTags.AssistTags.PARTNER };

        public override int[] Costs => new int[] { 3 };

        public override bool CostSame => true;

        public override AbstractCardPersistentSupport PersistentPool => new PaimonSupport();

        public class PaimonSupport : AbstractCardPersistentSupport
        {
            public override int MaxUseTimes => 2;

            public override string NameID => "paimon_support";

            public override string[] SpecialTags => new string[] { TCGBase.Tags.CardTags.AssistTags.PARTNER };

            public override Dictionary<string, PersistentTrigger> TriggerDic => new() { { TCGBase.Tags.SenderTags.ROUND_START, new PaimonTrigger() } };

            private class PaimonTrigger : PersistentTrigger
            {
                public void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
                {
                    persitent.AvailableTimes--;
                    Logger.Warning("大派触发了！");
                    if (me is PlayerTeam pt)
                    {
                        pt.AddDice(0);
                        pt.AddDice(0);
                    }
                    if (persitent.AvailableTimes == 0)
                    {
                        Logger.Warning("大派触发了！并且弃置！");
                    }
                }
            }


        }

    }
}
