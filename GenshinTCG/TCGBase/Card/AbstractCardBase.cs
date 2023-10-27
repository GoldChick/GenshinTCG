namespace TCGBase
{
    public abstract class AbstractCardBase
    {
        /// <summary>
        /// 卡牌的nameID(a-z+_+0-9),如"keqing"
        /// </summary>
        public abstract string NameID { get; }
        //TODO:既然结算都写在服务端，那没必要传了
        //需要显示的东西都写在客户端就好
        //@desperated

        /// <summary>
        /// 传递给客户端，在客户端显示，卡牌携带的特殊tag<br/>
        /// 一般自动产生
        /// </summary>
        public virtual string[] SpecialTags { get => Array.Empty<string>(); }
    }
    /// <summary>
    /// 可能会有用的客户端服务端分离？
    /// </summary>
    public abstract class AbstractCardServer : AbstractCardBase
    {

    }
}
