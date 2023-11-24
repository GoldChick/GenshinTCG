namespace TCGBase
{
    /// <summary>
    /// After使用Card的sender
    /// </summary>
    public class AfterUseCardSender : AbstractAfterActionSender
    {
        public override string SenderName => SenderTag.AfterUseCard.ToString();
        public AbstractCardAction Card { get; set; }
        public int[]? AdditionalTargetArgs { get; set; }

        internal AfterUseCardSender(int teamID, AbstractCardAction card, int[]? targetArgs = null) : base(teamID)
        {
            Card = card;
            AdditionalTargetArgs = targetArgs;
        }
    }
}
