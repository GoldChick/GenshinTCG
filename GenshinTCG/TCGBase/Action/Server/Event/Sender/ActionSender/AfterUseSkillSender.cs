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

        internal AfterUseSkillSender(int teamID, Character character, AbstractCardSkill skill) : base(teamID)
        {
            Character = character;
            Skill = skill;
        }
    }
}
