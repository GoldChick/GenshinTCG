namespace TCGBase
{
    /// <summary>
    /// 供减费使用
    /// 需要注意的是调用这个不代表会确认行动
    /// </summary>
    public class UseDiceFromSwitchSender : AbstractUseDiceSender
    {
        public override string SenderName => SenderTag.UseDiceFromSwitch.ToString();
        /// <summary>
        /// BeforeSwitch: source character
        /// </summary>
        public int Source { get; }
        /// <summary>
        /// BeforeSwitch: target character<br/>
        /// </summary>
        public int Target { get; }
        internal UseDiceFromSwitchSender(int teamID, int source, int target, bool isrealaction) : base(isrealaction, teamID)
        {
            Source = source;
            Target = target;
        }
    }
}
