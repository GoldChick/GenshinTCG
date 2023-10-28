namespace TCGBase
{
    public abstract class AbstractCardBase
    {
        /// <summary>
        /// 卡牌的nameID(a-z+_+0-9),如"keqing"
        /// </summary>
        public abstract string NameID { get; }
    }
    /// <summary>
    /// 可能会有用的客户端服务端分离？
    /// </summary>
    public abstract class AbstractCardServer : AbstractCardBase
    {

    }
}
