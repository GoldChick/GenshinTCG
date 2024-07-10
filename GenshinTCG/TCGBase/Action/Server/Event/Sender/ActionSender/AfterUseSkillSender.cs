namespace TCGBase
{
    /// <summary>
    /// After使用技能的sender<br/>
    /// 创建集成于GetSkillHandler()中
    /// </summary>
    public class AfterUseSkillSender : AbstractAfterActionSender, IPeristentSupplier, IMaySkillSupplier
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
    /// <summary>
    /// [使用技能后]的结算存在连续性这一特征<br/>
    /// 专门储存一下
    /// </summary>
    public record class AfterUseSkillRecord
    {
        public bool Damage { get; }
        public bool Element { get; }
        public bool Pierce { get; }
        public bool Reaction { get; }
    }
}
