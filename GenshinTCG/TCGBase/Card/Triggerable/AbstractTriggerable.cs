namespace TCGBase
{
    public abstract class AbstractTriggerable : INameable, INameSetable
    {
        public string Namespace { get; protected set; }
        /// <summary>
        /// 只有注册进Registry的NameID才是有用的，所以不要使用这个来当判断条件
        /// </summary>
        public abstract string NameID { get; protected set; }
        public abstract string Tag { get; }
        /// <summary>
        /// 结算或预结算一次效果<br/>
        /// 次数的减少需要自己维护
        /// </summary>
        /// <param name="persitent">当前触发效果的persistent对应的object,用来减少、增加次数</param>
        /// <param name="sender">信息的发送者,如打出的[牌],使用的[技能]</param>
        /// <param name="variable">可以被改写的东西,如[消耗的骰子们],[伤害] <b>(不应改变类型)</b></param>
        public abstract void Trigger(PlayerTeam me, Persistent persitent, AbstractSender sender, AbstractVariable? variable);
        protected AbstractTriggerable()
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
