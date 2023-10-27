using TCGBase;

namespace TCGMod
{
    public class Crystal : AbstractCardPersistentEffect
    {
        public override int InitialUseTimes => 1;
        public override int MaxUseTimes => 2;

        public override PersistentTriggerDictionary TriggerDic => new()
        {
            { SenderTag.HurtDecrease.ToString(),new PersistentYellowShield()}
        };

        public override string NameID => "结晶盾";
    }
}
