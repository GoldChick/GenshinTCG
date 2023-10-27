using TCGBase;

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

        public override PersistentTriggerDictionary TriggerDic => new() {
            {TCGBase.SenderTag.AfterAnyAction.ToString(), new PersistentSimpleConsume()} ,
            {SenderTag.AfterSwitch, new PersistentSimpleUpdate()}
             };
    }
}
