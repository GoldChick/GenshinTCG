using TCGCard;

namespace TCGMod
{
    /// <summary>
    /// 游戏开始时，为双方附属
    /// </summary>
    public class Floating : AbstractCardPersistentEffect
    {
        public override string NameID => "floating";
        public override int MaxUseTimes => 1;
        public override bool DeleteWhenUsedUp => false;

        public override Dictionary<string, PersistentTrigger> TriggerDic => new() {
            {TCGBase.Tags.SenderTags.AFTER_ANY_ACTION, new PersistentSimpleConsume()} ,
            {TCGBase.Tags.SenderTags.AFTER_SWITCH, new PersistentSimpleUpdate()}
             };
    }
}
