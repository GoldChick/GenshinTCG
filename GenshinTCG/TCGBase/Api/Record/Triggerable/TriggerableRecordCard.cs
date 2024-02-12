namespace TCGBase
{
    //TODO: 有目标的情况

    /// <summary>
    /// 卡牌打出时的效果
    /// </summary>
    public record TriggerableRecordCard : TriggerableRecordWithAction
    {
        public TriggerableRecordCard(List<ActionRecordBase> action) : base(TriggerableType.Card, action)
        {
        }
        public override AbstractCustomTriggerable GetTriggerable() => new TriggerableCard(this);
    }
}
