namespace TCGBase
{
    /// <summary>
    /// After使用技能的sender
    /// </summary>
    public class AfterUseSkillSender : AbstractAfterActionSender
    {
        public override string SenderName => SenderTag.AfterUseSkill.ToString();
        public int CharIndex { get; set; }
        public AbstractCardSkill Skill { get; set; }
        public int[]? AdditionalTargetArgs { get; set; }

        public AfterUseSkillSender(int teamID, int charIndex, AbstractCardSkill skill, int[]? targetArgs = null) : base(teamID)
        {
            CharIndex = charIndex;
            Skill = skill;
            AdditionalTargetArgs = targetArgs;
        }
    }
}
