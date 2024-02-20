namespace TCGBase
{
    public class CardEffect : AbstractCardBase, IEffect
    {
        public int MaxUseTimes { get; }
        public override int InitialUseTimes { get; }
        public virtual bool CustomDesperated { get; }
        public CardEffect(CardRecordEffect record) : base(record)
        {
            InitialUseTimes = record.InitialUseTimes;
            MaxUseTimes = record.MaxUseTimes;
            CustomDesperated = record.CustomDesperated;
        }
        public virtual void Update(PlayerTeam me, Persistent persistent)
        {
            persistent.Data.Clear();
            if (persistent.AvailableTimes < MaxUseTimes)
            {
                persistent.AvailableTimes = int.Min(MaxUseTimes, persistent.AvailableTimes + InitialUseTimes);
            }
        }
    }
}
