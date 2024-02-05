namespace TCGBase
{
    /// <summary>
    /// 触发[出战角色]技能效果的sender<br/>
    /// 需指定角色、技能的index<br/>
    /// </summary>
    public class ActionUseSkillSender : AbstractAfterActionSender
    {
        public override string SenderName => SenderTagInner.UseSkill.ToString();
        public int Character { get; set; }
        public int Skill { get; set; }

        public ActionUseSkillSender(int teamID, int character, int skill) : base(teamID)
        {
            Character = character;
            Skill = skill;
        }
    }
}
