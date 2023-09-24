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
        public ICardSkill Skill { get; set; }
        public int[]? AdditionalTargetArgs { get; set; }

        public UseSkillSender(int charIndex, ICardSkill skill, int[]? targetArgs = null)
        {
            CharIndex = charIndex;
            Skill = skill;
            AdditionalTargetArgs = targetArgs;
        }
    }
}
