using System;
using TCGBase;
using TCGUtil;

namespace Genshin3_3
{
    public class Paimon : AbstractCardSupport
    {
        public override string NameID => "paimon";

        public override int[] Costs => new int[] { 3 };

        public override bool CostSame => true;

        public override AbstractCardPersistentSupport PersistentPool => new PaimonSupport();

        public class PaimonSupport : AbstractCardPersistentSupport
        {
            public override int MaxUseTimes => 2;

            public override string NameID => "paimon_support";

            public override PersistentTriggerDictionary TriggerDic => new()
            {
                { SenderTag.RoundStart, (me,p,s,v)=>
                {
                    p.AvailableTimes--;
                    me.AddDice(0);
                    me.AddDice(0);
                }
                }
            };

            private class PaimonTrigger : PersistentTrigger
            {
                public override void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
                {
                    persitent.AvailableTimes--;
                    if (me is PlayerTeam pt)
                    {
                        pt.AddDice(0);
                        pt.AddDice(0);
                    }
                }
            }


        }

    }
}
