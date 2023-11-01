namespace TCGBase
{
    /// <summary>
    /// 供减费使用。
    /// 需要注意的是调用这个不代表会确认行动
    /// </summary>
    public class UseDiceFromSkillSender : AbstractUseDiceSender
    {
        public override string SenderName => SenderTag.UseDiceFromSkill.ToString();
        /// <summary>
        /// BeforeUseSkill: character index<br/>
        /// </summary>
        public int ChaIndex { get; }
        /// <summary>
        /// BeforeUseSkill: skill index<br/>
        /// </summary>
        public int SkillIndex { get; }
        internal UseDiceFromSkillSender(int teamID, int chaindex, int skillindex, bool isrealaction) : base(isrealaction, teamID)
        {
            ChaIndex = chaindex;
            SkillIndex = skillindex;
        }
    }
}
