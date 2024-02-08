namespace TCGBase
{
    public abstract class AbstractTriggerableSkill : ITriggerable, ICostable, ISkillable
    {
        public string Tag => SenderTagInner.UseSkill.ToString();
        public abstract SkillCategory SkillCategory { get; }
        public abstract CostInit Cost { get; }
        public abstract void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable);
        protected private AbstractTriggerableSkill()
        {
        }
    }
}