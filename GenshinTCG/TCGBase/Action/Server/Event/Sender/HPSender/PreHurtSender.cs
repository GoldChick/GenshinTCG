namespace TCGBase
{
    /// <summary>
    /// 结算增伤减伤所使用的sender ,包含根本source <see cref="IDamageSource"/>
    /// </summary>
    public class PreHurtSender : AbstractSender
    {
        public override string SenderName { get; }
        public AbstractCustomTriggerable RootSource { get; init; }
        /// <summary>
        /// 记录被打的角色头上的本来的元素，用来区分不同的结晶、扩散
        /// </summary>
        public int InitialElement { get; }
        internal PreHurtSender(int teamID, AbstractCustomTriggerable triggerable, SenderTag sender, int initialelement = -1) : base(teamID)
        {
            RootSource = triggerable;
            SenderName = sender.ToString();
            InitialElement = initialelement;
        }
    }
}
