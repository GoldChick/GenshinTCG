namespace TCGBase
{
    public class AfterOperationSender : SimpleSender, IModifier
    {
        public OperationType ActionType { get; }
        public bool RealAction => true;

        internal AfterOperationSender(int teamID, OperationType type) : base(teamID)
        {
            ActionType = type;
        }
    }
}
