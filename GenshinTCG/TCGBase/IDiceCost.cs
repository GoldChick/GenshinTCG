namespace TCGBase
{
    /// <summary>
    /// 支持"X黑"、"X白"、"{x1,x2...}{风，岩...}+X黑"、"{x1,x2...}{风，岩...}+X白"
    /// <br/>
    /// 不支持"X黑+X白"
    /// </summary>
    public interface IDiceCost
    {
        /// <summary>
        /// 需要的无色元素"minecraft:trival"是否需要颜色相同
        /// </summary>
        public bool SameDice { get; }
        /// <summary>
        /// 需要使用全称，如"("minecraft:electro",1)"<br/>
        /// 不写namespace会被直接识别为默认的"minecraft"<b>(未实现)</b>
        /// </summary>
        public Dictionary<string, int> Costs { get; }
    }
}
