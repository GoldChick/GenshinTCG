using TCGBase;

namespace Minecraft
{
    public class Effect_DendroCore : AbstractCardEffect
    {
        public override string NameID => "dendro_core";
        public override int MaxUseTimes => 1;

        public override PersistentTriggerDictionary TriggerDic => new()
        {
            { SenderTag.DamageIncrease, (me,p,s,v)=>
                {
                    if (s.TeamID==me.TeamIndex &&v is DamageVariable dv && (dv.Element==3 || dv.Element==4))
                    {
                        dv.Damage+=2;
                        p.AvailableTimes--;
                    }
                }
             }
        };

    }
}
