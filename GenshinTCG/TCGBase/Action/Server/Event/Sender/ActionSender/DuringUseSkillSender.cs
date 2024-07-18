namespace TCGBase
{
    /// <summary>
    /// [使用技能后]的结算存在连续性这一特征<br/>
    /// 用来监听
    /// </summary>
    public class DuringUseSkillSender : AbstractAfterActionSender, IPeristentSupplier, IMaySkillSupplier
    {
        public Dictionary<Persistent, EventPersistentHandler> Listeners { get; }
        public Character Character { get; set; }
        public AbstractTriggerable Skill { get; set; }
        Persistent IPeristentSupplier.Persistent => Character;
        ISkillable? IMaySkillSupplier.MaySkill => Skill as ISkillable;

        internal DuringUseSkillSender(int teamID, Character character, AbstractTriggerable skill) : base(teamID)
        {
            Character = character;
            Skill = skill;
            Listeners = new();
        }
    }
}
