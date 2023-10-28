using TCGBase;
namespace Genshin3_3
{
    public class 参量质变仪 : AbstractCardSupport
    {
        public override AbstractCardPersistentSupport PersistentPool => new 参量质变仪_P();

        public override int[] Costs => new int[] { 2 };

        public override bool CostSame => false;

        public override string NameID => "参量质变仪";
        private class 参量质变仪_P : AbstractCardPersistentSupport
        {
            public override int MaxUseTimes => 0;
            public override bool DeleteWhenUsedUp => false;
            public override PersistentTriggerDictionary TriggerDic => new()
            {
                { SenderTag.BeforeUseSkill.ToString(),(me,p,s,v)=>p.Data=1},
                { SenderTag.AfterHurt.ToString(),(me,p,s,v)=>
                {
                    if (p.Data!=null && p.Data.Equals(1)&& s is HurtSender hs && hs.Element>0)
                    {
                        p.Data=2;
                    }
                }},
                { SenderTag.AfterUseSkill.ToString(),(me,p,s,v)=>
                {
                    if (p.Data!=null && p.Data.Equals(2))
                    {
                        p.AvailableTimes++;
                        if (p.AvailableTimes==3)
                        {
                            p.Active=false;
                            for (int i = 0; i < 7; i++)
                            {
                                me.Random.Next(7);
                                //TODO:爆质变仪

                            }
                           if (me is PlayerTeam pt)
    {
                                    pt.AddDice(0);
                                    pt.AddDice(0);
                                    pt.AddDice(0);
    }
                        }
                    }
                    p.Data=0;
                }}
            };

            public override string NameID => "参量质变仪_P";
        }
    }
}
