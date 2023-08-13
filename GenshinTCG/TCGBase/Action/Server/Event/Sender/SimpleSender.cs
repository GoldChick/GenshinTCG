namespace TCGBase
{
    public class SimpleSender : AbstractSender
    {
        public override string SenderName { get; }

        public SimpleSender(string sender)
        {
            SenderName = sender;
        }
    }
}
