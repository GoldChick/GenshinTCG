namespace TCGBase
{
    public abstract class AbstractUseDiceSender : AbstractSender
    {
        public bool IsRealAction { get; }
        public AbstractUseDiceSender(bool isrealaction, int teamID) : base(teamID)
        {
            IsRealAction = isrealaction;
        }
    }
}
