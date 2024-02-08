namespace TCGBase
{
    public class AfterOperationSender : AbstractSender
    {
        public override string SenderName => SenderTag.AfterOperation.ToString();
        public OperationType ActionType { get; }
        internal AfterOperationSender(int teamID, OperationType type) : base(teamID)
        {
            ActionType = type;
        }
    }
}
