namespace TCGBase
{
    public class SwitchSender : AbstractSender
    {
        public override string SenderName => Tags.SenderTags.SWITCH;
        /// <summary>
        /// 切换到的角色在队伍中的index
        /// </summary>
        public int SwitchTarget { get; init; }
        public SwitchSender(int target) 
        {
            SwitchTarget= target;
        }
    }
}
