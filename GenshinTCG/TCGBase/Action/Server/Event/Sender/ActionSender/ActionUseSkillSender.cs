namespace TCGBase
{
    /// <summary>
    /// 触发[出战角色]技能效果的sender<br/>
    /// 需指定角色、技能的index<br/>
    /// </summary>
    public class ActionUseSkillSender : AbstractAfterActionSender
    {
        public override string SenderName => SenderTag.AfterUseSkill.ToString();
        public Character Character { get; set; }
        public int Skill { get; set; }

        public ActionUseSkillSender(int teamID, Character character, int skill) : base(teamID)
        {
            Character = character;
            Skill = skill;
        }
    }
}
