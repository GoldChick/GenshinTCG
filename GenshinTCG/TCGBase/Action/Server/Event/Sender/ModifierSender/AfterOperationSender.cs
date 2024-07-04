namespace TCGBase
{
    public class AfterOperationSender : AbstractSender, IModifier
    {
        public override string SenderName => SenderTag.AfterOperation.ToString();
        public OperationType ActionType { get; }
        public bool RealAction => true;

        internal AfterOperationSender(int teamID, OperationType type) : base(teamID)
        {
            ActionType = type;
        }
    }
}
