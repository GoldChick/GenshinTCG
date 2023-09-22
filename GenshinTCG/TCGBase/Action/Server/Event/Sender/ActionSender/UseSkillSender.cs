using TCGCard;

namespace TCGBase
{
    /// <summary>
    /// 使用技能的sender，参数为ICardSkill
    /// </summary>
    public class UseSkillSender : AbstractAfterActionSender
    {
        public override string SenderName => Tags.SenderTags.AFTER_USE_SKILL;
        public ICardSkill Skill { get; set; }

        public UseSkillSender(ICardSkill skill)
        {
            Skill = skill;
        }
    }
}
