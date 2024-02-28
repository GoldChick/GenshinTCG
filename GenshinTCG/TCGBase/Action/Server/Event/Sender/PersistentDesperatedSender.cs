namespace TCGBase
{
    public class PersistentDesperatedSender : AbstractSender
    {
        public override string SenderName => SenderTag.AfterEffectDesperated.ToString();
        public int Region { get; }
        public ICard Persistent { get; }
        internal PersistentDesperatedSender(int teamID, int region, ICard persistent) : base(teamID)
        {
            Region = region;
            Persistent = persistent;
        }
    }
}
