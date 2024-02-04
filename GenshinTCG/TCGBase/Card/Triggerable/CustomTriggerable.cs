namespace TCGBase
{
    public abstract class AbstractCustomTriggerable : ITriggerable, INameable
    {
        public string Namespace => (GetType().Namespace ?? "minecraft").ToLower();
        public abstract string NameID { get; }
        public abstract string Tag { get; }
        public abstract void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable);
    }
    public abstract class AbstractCustomSkillTriggerable : AbstractSkillTriggerable
    {
        /// <summary>
        /// 这里的persistent其实是character，需要的话可以自行取用
        /// </summary>
        public abstract void AfterUseAction(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable);
        /// <summary>
        /// CustomSkillTriggerable已经自动封装Trigger，只需要在AfterUseAction中写出需要发生的事情即可
        /// </summary>
        public sealed override void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
            => TriggerablePreset.GetSkillHandler(AfterUseAction)?.Invoke(me, persitent, sender, variable);
    }
}
