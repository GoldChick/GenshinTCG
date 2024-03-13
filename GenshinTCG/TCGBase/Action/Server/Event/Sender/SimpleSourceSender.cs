namespace TCGBase
{
    /// <summary>
    /// 携带发出该sender的persistent作为参数，可自定义name的sender
    /// </summary>
    public class SimpleSourceSender : AbstractSender, IPeristentSupplier
    {
        public override string SenderName { get; }

        public Persistent Source { get; }

        Persistent IPeristentSupplier.Persistent => Source;

        public SimpleSourceSender(int teamID, SenderTag sender, Persistent source) : this(teamID, sender.ToString(), source) { }
        public SimpleSourceSender(int teamID, string sender, Persistent source) : base(teamID)
        {
            SenderName = sender;
            Source = source;
        }
    }
}
