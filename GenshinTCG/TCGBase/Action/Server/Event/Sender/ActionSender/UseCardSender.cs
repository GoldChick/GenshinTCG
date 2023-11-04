namespace TCGBase
{
    /// <summary>
    /// 使用技能的sender，参数为ICardSkill
    /// </summary>
    public class UseCardSender : AbstractAfterActionSender
    {
        public override string SenderName => SenderTag.AfterUseCard.ToString();
        public AbstractCardAction Card { get; set; }
        public int[]? AdditionalTargetArgs { get; set; }

        public UseCardSender(int teamID, AbstractCardAction card, int[]? targetArgs = null) : base(teamID)
        {
            Card = card;
            AdditionalTargetArgs = targetArgs;
        }
    }
}
