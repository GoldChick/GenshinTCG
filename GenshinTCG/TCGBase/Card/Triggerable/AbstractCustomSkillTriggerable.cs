namespace TCGBase
{
    public abstract class AbstractCustomSkillTriggerable : AbstractTriggerable, ICostable, ISkillable
    {
        public sealed override string Tag => SenderTagInner.UseSkill.ToString();
        public abstract SkillCategory SkillCategory { get; }
        public abstract CostInit Cost { get; }
        /// <summary>
        /// 这里的persistent其实是character，需要的话可以自行取用
        /// </summary>
        public abstract void AfterUseAction(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable);
        /// <summary>
        /// CustomSkillTriggerable已经自动封装Trigger，只需要在AfterUseAction中写出需要发生的事情即可
        /// </summary>
        public sealed override void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable)
            => TriggerablePreset.GetSkillHandler(AfterUseAction)?.Invoke(me, persitent, sender, variable);
    }
}
