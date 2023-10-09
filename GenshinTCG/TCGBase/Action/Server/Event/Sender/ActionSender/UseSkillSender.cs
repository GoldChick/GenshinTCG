using TCGCard;

namespace TCGBase
{
    /// <summary>
    /// 使用技能的sender，参数为ICardSkill
    /// </summary>
    public class UseSkillSender : AbstractAfterActionSender
    {
        public override string SenderName => Tags.SenderTags.AFTER_USE_SKILL;
        public int CharIndex { get; set; }
        public AbstractCardSkill Skill { get; set; }
        public int[]? AdditionalTargetArgs { get; set; }

        public UseSkillSender(int teamID, int charIndex, AbstractCardSkill skill, int[]? targetArgs = null) : base(teamID)
        {
            CharIndex = charIndex;
            Skill = skill;
            AdditionalTargetArgs = targetArgs;
        }
    }
}
