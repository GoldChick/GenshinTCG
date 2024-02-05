namespace TCGBase
{
    public abstract class AbstractCardEffect : AbstractCardBase, IEffect
    {
        private protected AbstractCardEffect(CardRecordBase record) : base(record)
        {
        }

        public virtual bool CustomDesperated { get => false; }
        public override int InitialUseTimes => MaxUseTimes;
        public abstract int MaxUseTimes { get; }

        public virtual void Update(PlayerTeam me, Persistent persistent)
        {
            persistent.Data = null;
            if (persistent.AvailableTimes < MaxUseTimes)
            {
                persistent.AvailableTimes = int.Min(MaxUseTimes, persistent.AvailableTimes + InitialUseTimes);
            }
        }
    }
}
