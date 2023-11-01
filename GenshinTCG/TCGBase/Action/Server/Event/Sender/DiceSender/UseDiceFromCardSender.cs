namespace TCGBase
{
    /// <summary>
    /// 供减费使用。
    /// 需要注意的是调用这个不代表会确认行动
    /// </summary>
    public class UseDiceFromCardSender : AbstractUseDiceSender
    {
        public override string SenderName => SenderTag.UseDiceFromCard.ToString();
        /// <summary>
        /// BeforeUseCard: card index
        /// </summary>
        public int CardIndex { get; }
        internal UseDiceFromCardSender(int teamID, int cardindex, bool isrealaction = false) : base(isrealaction, teamID)
        {
            CardIndex = cardindex;
        }
    }
}
