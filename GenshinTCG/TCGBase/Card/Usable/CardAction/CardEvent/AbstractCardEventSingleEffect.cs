namespace TCGBase
{
    public abstract class AbstractCardEventSingleEffect : AbstractCardEvent, ICardPersistent
    {
        public virtual int InitialUseTimes => MaxUseTimes;

        public abstract int MaxUseTimes { get; }

        public int Variant { get; protected set; }

        public virtual bool CustomDesperated => false;

        public abstract PersistentTriggerDictionary TriggerDic { get; }

        public void Update<T>(Persistent<T> persistent) where T : ICardPersistent
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
