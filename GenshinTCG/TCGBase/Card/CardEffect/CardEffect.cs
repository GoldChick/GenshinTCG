namespace TCGBase
{
    internal class CardEffect : AbstractCardEffect
    {
        public override string Namespace { get; }
        public override int InitialUseTimes { get; }
        public override int MaxUseTimes { get; }
        public CardEffect(CardRecordEffect record, string @namespace) : base(record)
        {
            InitialUseTimes = record.InitialUseTimes;
            MaxUseTimes = record.MaxUseTimes;
            Namespace = @namespace;
        }
    }
}
