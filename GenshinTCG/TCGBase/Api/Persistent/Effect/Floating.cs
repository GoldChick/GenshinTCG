namespace TCGBase
{
    /// <summary>
    /// 游戏开始时，为双方附属
    /// </summary>
    public class Floating : AbstractCardPersistentEffect
    {
        public override int MaxUseTimes => 1;
        public override bool CustomDesperated => false;

        public override PersistentTriggerDictionary TriggerDic => new() {
            {TCGBase.SenderTag.AfterAnyAction.ToString(), new PersistentSimpleConsume()} ,
            {SenderTag.AfterSwitch, new PersistentSimpleUpdate()}
             };
    }
}
