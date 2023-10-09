namespace TCGCard
{
    public abstract class AbstractCardBase
    {
        /// <summary>
        /// 卡牌的nameID(a-z+_+0-9),如"keqing"
        /// </summary>
        public abstract string NameID { get; }
        /// <summary>
        /// 默认携带的各种tag<br/>
        /// 成员形如(minecraft:Nation,minecraft:Natlan)<br/>
        /// 或者(minecraft:)
        /// </summary>
        public abstract string[] Tags { get; }
        //TODO:@deprecated
        //并不是所有卡牌都需要这些tag
    }
    /// <summary>
    /// 可能会有用的客户端服务端分离？
    /// </summary>
    public abstract class AbstractCardServer : AbstractCardBase
    {

    }
}
