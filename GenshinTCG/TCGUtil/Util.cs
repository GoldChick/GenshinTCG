namespace TCGUtil
{
    /// <summary>
    /// 适用于只需要在控制台中打印状态的对象
    /// </summary>
    public interface IPrintable
    {
        /// <summary>
        /// 打印出这个class需要在控制台显示的内容
        /// </summary>
        void Print();
    }
    /// <summary>
    /// 适用于需要特殊的序列化方式的对象
    /// </summary>
    public interface IJsonable : IPrintable
    {
        /// <summary>
        /// 返回经特殊规则序列化后的Json
        /// </summary>
        string Json();
    }
    public interface IDetailable : IJsonable
    {
        /// <summary>
        /// 返回经特殊规则更加详细序列化后的Json
        /// </summary>
        string JsonDetail();
    }
}