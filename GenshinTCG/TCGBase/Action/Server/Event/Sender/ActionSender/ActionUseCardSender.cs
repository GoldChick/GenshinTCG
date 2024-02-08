namespace TCGBase
{
    public class ActionUseCardSender : AbstractAfterActionSender
    {
        public override string SenderName => SenderTagInner.UseCard.ToString();
        public int Card { get; }
        public int[] Args { get; }
        public ActionUseCardSender(int teamID, int card, int[] args) : base(teamID)
        {
            Card = card;
            Args = args;
        }
    }
}
