namespace TCGBase
{
    public class AfterSimpleActionSender : AbstractSender
    {
        public override string SenderName { get; }
        protected AfterSimpleActionSender(ActionType type, int teamID) : base(teamID)
        {
            SenderName = type.ToSenderTags().ToString();
        }
    }
}
