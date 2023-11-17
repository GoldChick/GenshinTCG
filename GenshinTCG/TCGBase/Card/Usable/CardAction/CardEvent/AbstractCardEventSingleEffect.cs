namespace TCGBase
{
    public abstract class AbstractCardEventSingleEffect : AbstractCardEvent, ICardPersistnet
    {
        public virtual int InitialUseTimes => MaxUseTimes;

        public abstract int MaxUseTimes { get; }

        public int Variant { get; protected set; }

        public virtual bool CustomDesperated => false;

        public abstract PersistentTriggerDictionary TriggerDic { get; }

        public int[] Info(AbstractPersistent p) => new int[] { p.AvailableTimes };

        public void Update<T>(Persistent<T> persistent) where T : ICardPersistnet
        {
            persistent.Data = null;
            persistent.AvailableTimes = int.Max(persistent.AvailableTimes, MaxUseTimes);
        }
        public override void AfterUseAction(PlayerTeam me, int[] targetArgs)
        {
            me.AddPersistent(this);
        }
    }
}
