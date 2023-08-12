namespace TCGBase
{
    public abstract class AbstractNetCardSet
    {
    }
    public class PlayerNetCardSet : AbstractNetCardSet
    {
        /// <summary>
        /// size=3
        /// </summary>
        public string[] Characters { get; init; }
        /// <summary>
        /// size=30
        /// </summary>
        public string[] ActionCards { get; init; }
    }
    public class PlayerDetailNetCardSet : PlayerNetCardSet
    {
        /// <summary>
        /// size=4
        /// </summary>
        public string[] Supports { get; }
        /// <summary>
        /// size=4
        /// </summary>
        public string[] Summons { get; }
    }
    /// <summary>
    /// 固定行动的电脑卡组
    /// Characters[x][y]代表第x+1波的第y+1张角色卡（场上同时最多存在4个角色，全部被击倒之后进入下一波）
    /// </summary>
    public class EnvironmentNetCardSet : AbstractNetCardSet
    {
        public string[][] Characters { get; }
        public string[][] Actions { get; }
    }

}
