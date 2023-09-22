namespace TCGBase
{
    /// <summary>
    /// 切换角色的sender，参数为起始index和切换到的index
    /// </summary>
    public class SwitchSender : AbstractAfterActionSender
    {
        public override string SenderName => Tags.SenderTags.AFTER_SWITCH;
        /// <summary>
        /// 原先的index
        /// </summary>
        public int Initial { get; init; }
        /// <summary>
        /// 切换到的角色在队伍中的index
        /// </summary>
        public int SwitchTarget { get; init; }
        public SwitchSender(int init, int target)
        {
            Initial = init;
            SwitchTarget = target;
        }
    }
}
