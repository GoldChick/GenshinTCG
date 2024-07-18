namespace TCGBase
{
    public record CardRecordListener : CardRecordBase
    {
        public CardRecordListener(List<TriggerableRecordBase> skillList) : base(true, CardType.Listener, skillList, null, null)
        {
        }
        public CardListener GetCard() => new CardListener(this);
    }
}
