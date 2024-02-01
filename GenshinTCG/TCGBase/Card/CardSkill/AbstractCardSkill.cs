namespace TCGBase
{
    /// <summary>
    /// 分别表示[非技能][A][E][Q][被动]
    /// </summary>
    public enum SkillCategory
    {
        N,
        A,
        E,
        Q,
        P
    }
    public abstract class AbstractCardSkill : IDamageSource, ICostable, ISkillable
    {
        //by the way,我不认为被隐藏的非[准备]非[被动]技能有存在的必要
        /// <summary>
        /// 是否为隐藏，隐藏技能无法触发[使用技能后]<br/>
        /// [准备技能]和[被动技能]默认不触发
        /// </summary>
        public virtual bool Hidden => true;
        public abstract CostInit Cost { get; }
        public abstract SkillCategory SkillCategory { get; }
        public abstract void AfterUseAction(AbstractTeam me, Character c);
    }
    public abstract class AbstractSkillTrigger : IPersistentTrigger, ICostable, ISkillable
    {
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

        public override void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {

        }
    }
}