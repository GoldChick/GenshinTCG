namespace TCGBase
{
    public abstract class AbstractCardSet
    {
        public abstract bool IsValid();
    }
    public class PlayerCardSet : AbstractCardSet
    {
        /// <summary>
        /// size=3
        /// </summary>
        public string[] Characters { get; init; }
        /// <summary>
        /// size=30
        /// </summary>
        public string[] ActionCards { get;  init; }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
    public class PlayerDetailCardSet : PlayerCardSet
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
    public class EnvironmentCardSet : AbstractCardSet
    {
        public string[][] Characters { get; }
        public string[][] Actions { get; }
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }

}
