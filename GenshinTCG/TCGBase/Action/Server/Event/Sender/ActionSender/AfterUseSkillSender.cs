namespace TCGBase
{
    /// <summary>
    /// After使用技能的sender<br/>
    /// 创建集成于GetSkillHandler()中
    /// </summary>
    public class AfterUseSkillSender : AbstractAfterActionSender
    {
        public override string SenderName => SenderTag.AfterUseSkill.ToString();
        public Character Character { get; set; }
        public AbstractCustomTriggerable Skill { get; set; }

        internal AfterUseSkillSender(int teamID, Character character, AbstractCustomTriggerable skill) : base(teamID)
        {
            Character = character;
            Skill = skill;
        }
    }
}
