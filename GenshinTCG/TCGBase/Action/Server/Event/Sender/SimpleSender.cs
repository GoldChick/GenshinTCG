namespace TCGBase
{
    /// <summary>
    /// 不携带任何参数，只有name的sender
    /// </summary>
    public class SimpleSender : AbstractSender
    {
        public override string SenderName { get; }

        public SimpleSender(string sender)
        {
            SenderName = sender;
        }
    }
}
