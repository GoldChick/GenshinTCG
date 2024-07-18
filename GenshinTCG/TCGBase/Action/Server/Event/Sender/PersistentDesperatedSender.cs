namespace TCGBase
{
    public class PersistentDesperatedSender : SimpleSender
    {
        public int Region { get; }
        public ICard Persistent { get; }
        internal PersistentDesperatedSender(int teamID, int region, ICard persistent) : base(teamID)
        {
            Region = region;
            Persistent = persistent;
        }
    }
}
