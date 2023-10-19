using TCGBase;
using TCGCard;

namespace TCGMod
{
    public class Burning : AbstractCardPersistentSummon
    {
        public override int MaxUseTimes => 2;

        public override Dictionary<string, PersistentTrigger> TriggerDic => new()
        {
            { Tags.SenderTags.ROUND_OVER,new((me,p,s,v)=>
                {
                    me.Hurt(new DamageVariable(3,1,DamageSource.Summon,0));
                    p.AvailableTimes--;
                }
            )}
        }
        ;

        public override string NameID => "燃烧烈焰";
    }
}
