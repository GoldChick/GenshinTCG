namespace TCGInfo
{
    /// <summary>
    /// 其中的类全部用于序列化与反序列化
    /// 只传递需要在客户端显示的内容
    /// </summary>
    public struct EffectInfo
    {
        public string EffectID { get; set; }
        public int LeftTimes { get; set; }
    }
}
