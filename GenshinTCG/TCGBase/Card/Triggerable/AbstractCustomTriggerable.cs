namespace TCGBase
{
    public abstract class AbstractCustomTriggerable : ITriggerable, INameable, INameSetable
    {
        public string Namespace { get; protected set; }
        /// <summary>
        /// 只有注册进Registry的NameID才是有用的，所以不要使用这个来当判断条件
        /// </summary>
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
