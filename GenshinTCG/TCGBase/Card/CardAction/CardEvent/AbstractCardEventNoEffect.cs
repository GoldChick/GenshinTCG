namespace TCGBase
{
    /// <summary>
    /// 事件牌，比较笼统，如产生buff、战斗行动、操作召唤物等<br/>
    /// </summary>
    public abstract class AbstractCardEventNoEffect : AbstractCardEvent
    {
        public override int MaxUseTimes => 0;
        public override PersistentTriggerDictionary TriggerDic => new();
    }
}
