namespace TCGBase
{
    public abstract class AbstractSkillTriggerable : ITriggerable, ICostable, ISkillable
    {
        public string Tag => SenderTagInner.UseSkill.ToString();
        public abstract SkillCategory SkillCategory { get; }
        public abstract CostInit Cost { get; }
        public abstract void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable);
        protected private AbstractSkillTriggerable()
        {
        }
    }
}