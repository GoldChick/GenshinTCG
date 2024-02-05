namespace TCGBase
{
    internal class CardEffect : AbstractCardEffect
    {
        public override int InitialUseTimes { get; }
        public override int MaxUseTimes { get; }
        public CardEffect(CardRecordEffect record) : base(record)
        {
            InitialUseTimes = record.InitialUseTimes;
            MaxUseTimes = record.MaxUseTimes;
        }
    }
}
