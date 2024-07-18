namespace TCGBase
{
    /// <summary>
    /// 触发[出战角色]技能效果的sender<br/>
    /// 需指定角色、技能的index<br/>
    /// </summary>
    public class ActionUseSkillSender : SimpleSender, ITriggerableIndexSupplier
    {
        public int Character { get; set; }
        public int Skill { get; set; }
        int ITriggerableIndexSupplier.TriggerableIndex => Skill;
        int ITriggerableIndexSupplier.SourceIndex => Character;

        public ActionUseSkillSender(int teamID, int character, int skill) : base(teamID)
        {
            Character = character;
            Skill = skill;
        }
    }
}
