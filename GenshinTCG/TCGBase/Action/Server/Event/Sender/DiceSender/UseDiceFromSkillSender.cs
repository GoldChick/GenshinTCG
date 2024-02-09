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
        /// BeforeUseSkill: character<br/>
        /// </summary>
        public Character Character { get; }
        /// <summary>
        /// BeforeUseSkill: skill<br/>
        /// </summary>
        public ITriggerable Skill { get; }
        internal UseDiceFromSkillSender(int teamID, Character cha, ITriggerable skill, bool isrealaction) : base(isrealaction, teamID)
        {
            Character = cha;
            Skill = skill;
        }
    }
}
