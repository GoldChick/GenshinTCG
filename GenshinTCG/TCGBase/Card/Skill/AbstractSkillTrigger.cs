namespace TCGBase
{
    /// <summary>
    /// 分别表示[非技能/被动][A][E][Q]
    /// </summary>
    public enum SkillCategory
    {
        P,
        A,
        E,
        Q,
    }
    public abstract class AbstractSkillTrigger : ITriggerable, ICostable, ISkillable
    {
        //by the way,我不认为被隐藏的非[准备]非[被动]技能有存在的必要
        /// <summary>
        /// 是否为隐藏，隐藏技能无法触发[使用技能后]<br/>
        /// [准备技能]和[被动技能]默认不触发
        /// </summary>
        public virtual bool Hidden => false;

        public string Tag => SenderTagInner.Use.ToString();

        public abstract SkillCategory SkillCategory { get; }

        public abstract CostInit Cost { get; }

        public abstract void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable);
    }
    public class SkillTrigger : AbstractSkillTrigger
    {
        public SkillTrigger(SkillCategory category, CostInit cost)
        {
            SkillCategory = category;
            Cost = cost;
        }
        public override SkillCategory SkillCategory { get; }

        public override CostInit Cost { get; }
        //Action<AbstractTeam, AbstractPersistent, AbstractSender, AbstractVariable?>
        public EventPersistentHandler? Action { get; internal set; }
        public override void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable) => Action?.Invoke(me, persitent, sender, variable);
    }
}