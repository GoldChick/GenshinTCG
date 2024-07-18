namespace TCGBase
{
    /// <summary>
    /// 携带发出该sender的persistent作为参数，可自定义name的sender
    /// </summary>
    public class SimpleSourceSender : SimpleSender, IPeristentSupplier
    {
        public Persistent Source { get; }

        Persistent IPeristentSupplier.Persistent => Source;

        public SimpleSourceSender(int teamID, Persistent source) : base(teamID)
        {
            Source = source;
        }
    }
}
