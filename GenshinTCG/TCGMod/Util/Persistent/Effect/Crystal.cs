using TCGBase;
using TCGCard;

namespace TCGMod
{
    public class Crystal : AbstractCardPersistentEffect
    {
        public override int InitialUseTimes => 1;
        public override int MaxUseTimes => 2;

        public override PersistentTriggerDictionary TriggerDic => new()
        {
            { Tags.SenderTags.HURT_DECREASE,new PersistentYellowShield()}
        };

        public override string NameID => "结晶盾";
    }
}
