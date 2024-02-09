namespace TCGBase
{
    public abstract class AbstractCustomTriggerable : ITriggerable, INameable, INameSetable
    {
        public string Namespace { get; protected set; }
        public abstract string NameID { get; protected set; }
        public abstract string Tag { get; }
        public abstract void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable);
        protected AbstractCustomTriggerable()
        {
            Namespace = (GetType().Namespace ?? "minecraft").ToLower();
        }
        void INameSetable.SetName(string @namespace, string nameid)
        {
            Namespace = @namespace;
            NameID = nameid;
        }
    }
}
