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
}