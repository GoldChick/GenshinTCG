namespace TCGBase
{
    public abstract class AbstractCustomTriggerable : ITriggerable, INameable
    {
        public string Namespace  => (GetType().Namespace ?? "minecraft").ToLower();
        public abstract string NameID { get; }
        public abstract string Tag { get; }
        public abstract void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable);
    }

    public abstract class AbstractSkillTriggerable : ITriggerable, ICostable, ISkillable
    {
        //by the way,我不认为被隐藏的非[准备]非[被动]技能有存在的必要
        /// <summary>
        /// 是否为隐藏，隐藏技能无法触发[使用技能后]<br/>
        /// [准备技能]和[被动技能]默认不触发
        /// </summary>
        public virtual bool Hidden => false;

        public string Tag => SenderTagInner.UseSkill.ToString();

        public abstract SkillCategory SkillCategory { get; }

        public abstract CostInit Cost { get; }
        public abstract void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable);
    }
    public class SkillTriggerable : AbstractSkillTriggerable
    {
        public SkillTriggerable(SkillCategory category, CostInit cost)
        {
            SkillCategory = category;
            Cost = cost;
        }
        public override SkillCategory SkillCategory { get; }
        public override CostInit Cost { get; }
        public EventPersistentHandler? Action { get; internal set; }
        public override void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable) => Action?.Invoke(me, persitent, sender, variable);
    }
}