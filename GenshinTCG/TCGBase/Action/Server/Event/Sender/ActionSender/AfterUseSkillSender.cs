namespace TCGBase
{
    /// <summary>
    /// After使用技能的sender<br/>
    /// 创建集成于GetSkillHandler()中
    /// </summary>
    public class AfterUseSkillSender : AbstractAfterActionSender, IPeristentSupplier,IMaySkillSupplier
    {
        public override string SenderName => SenderTag.AfterUseSkill.ToString();
        public Character Character { get; set; }
        public AbstractTriggerable Skill { get; set; }

        Persistent IPeristentSupplier.Persistent => Character;
        ISkillable? IMaySkillSupplier.MaySkill => Skill as ISkillable;

        internal AfterUseSkillSender(int teamID, Character character, AbstractTriggerable skill) : base(teamID)
        {
            Character = character;
            Skill = skill;
        }
    }
}
