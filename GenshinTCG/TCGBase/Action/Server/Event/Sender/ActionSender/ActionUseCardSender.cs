namespace TCGBase
{
    public class ActionUseCardSender : AbstractAfterActionSender
    {
        public override string SenderName => SenderTagInner.UseCard.ToString();
        public int Card { get; set; }

        public ActionUseCardSender(int teamID,  int card) : base(teamID)
        {
            Card = card;
        }
    }
}
