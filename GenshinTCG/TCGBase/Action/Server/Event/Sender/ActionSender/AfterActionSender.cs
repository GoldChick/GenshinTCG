namespace TCGBase
{
    public class AfterActionSender : AbstractSender
    {
        public override string SenderName => SenderTag.AfterAnyAction.ToString();
        public ActionType ActionType { get; }
        internal AfterActionSender(int teamID, ActionType type) : base(teamID)
        {
            ActionType = type;
        }
    }
}
