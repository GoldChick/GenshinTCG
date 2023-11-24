namespace TCGBase
{
    public class PersistentDesperatedSender : AbstractSender
    {
        public override string SenderName => SenderTag.AfterPersistentOtherDesperated.ToString();
        public int Region { get; }
        public ICardPersistent Persistent { get; }
        internal PersistentDesperatedSender(int teamID, int region, ICardPersistent persistent) : base(teamID)
        {
            Region = region;
            Persistent = persistent;
        }
    }
}
