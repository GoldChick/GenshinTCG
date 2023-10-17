namespace TCGCard
{
    public abstract class AbstractCardBase
    {
        /// <summary>
        /// 卡牌的nameID(a-z+_+0-9),如"keqing"
        /// </summary>
        public abstract string NameID { get; }
        /// <summary>
        /// 传递给客户端，在客户端显示，卡牌携带的特殊tag<br/>
        /// 成员形如(minecraft:Nation,minecraft:Natlan)<br/>
        /// 或者(minecraft:)<br/>
        /// 一般自动产生
        /// </summary>
        public abstract string[] SpecialTags { get; }
    }
    /// <summary>
    /// 可能会有用的客户端服务端分离？
    /// </summary>
    public abstract class AbstractCardServer : AbstractCardBase
    {

    }
}
