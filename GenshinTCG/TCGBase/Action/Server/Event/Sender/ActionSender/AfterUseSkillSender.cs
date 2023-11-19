namespace TCGBase
{
    /// <summary>
    /// After使用技能的sender
    /// </summary>
    public class AfterUseSkillSender : AbstractAfterActionSender
    {
        public override string SenderName => SenderTag.AfterUseSkill.ToString();
        public Character Character { get; set; }
        public AbstractCardSkill Skill { get; set; }
        public int[]? AdditionalTargetArgs { get; set; }

        public AfterUseSkillSender(int teamID, Character character, AbstractCardSkill skill, int[]? targetArgs = null) : base(teamID)
        {
            Character = character;
            Skill = skill;
            AdditionalTargetArgs = targetArgs;
        }
    }
}
