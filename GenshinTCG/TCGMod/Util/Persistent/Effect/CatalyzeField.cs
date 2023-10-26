using TCGBase;
using TCGCard;

namespace TCGMod
{
    public class CatalyzeField : AbstractCardPersistentEffect
    {
        public override int MaxUseTimes => 2;

        public override PersistentTriggerDictionary TriggerDic => new()
        {
            {Tags.SenderTags.DAMAGE_INCREASE,(me,p,s,v)=>
                {
                    if (s.TeamID==me.TeamIndex && v is DamageVariable dv && dv.TargetIndex==me.CurrCharacter && (dv.Element==4||dv.Element==6))
                    {
                        dv.Damage++;
                        p.AvailableTimes--;
                    }
                }
            }

        };

        public override string NameID => "激化领域";
    }
}
