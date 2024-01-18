namespace TCGBase
{
    /// <summary>
    /// 触发[出战角色]技能效果的sender
    /// </summary>
    public class ActionUseSkillSender : AbstractAfterActionSender
    {
        public override string SenderName => SenderTag.AfterUseSkill.ToString();
        public Character Character { get; set; }
        public AbstractCardSkill Skill { get; set; }
        public int[]? AdditionalTargetArgs { get; set; }

        internal ActionUseSkillSender(int teamID, Character character, AbstractCardSkill skill, int[]? targetArgs = null) : base(teamID)
        {
            Character = character;
            Skill = skill;
            AdditionalTargetArgs = targetArgs;
        }
    }
}
