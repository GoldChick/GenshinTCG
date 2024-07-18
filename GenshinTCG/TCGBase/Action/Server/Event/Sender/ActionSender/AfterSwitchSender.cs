namespace TCGBase
{
    /// <summary>
    /// After切换角色的sender，参数为起始index和切换到的index
    /// </summary>
    public class AfterSwitchSender : AbstractAfterActionSender
    {
        /// <summary>
        /// 原先的index
        /// </summary>
        public int Source { get; init; }
        /// <summary>
        /// 切换到的角色在队伍中的index
        /// </summary>
        public int Target { get; init; }
        internal AfterSwitchSender(int teamID, int source, int target) : base(teamID)
        {
            Source = source;
            Target = target;
        }
    }
}
