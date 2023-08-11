namespace TCGCard
{
    public enum TargetEnum
    {
        Character_Me,
        Character_Enemy,
        Summon,
        Support
    }
    /// <summary>
    /// 实现这个接口的卡牌使用时除了骰子，还必须需要选择另外的目标
    /// 支援牌不需要实现这种东西，检测将在内部进行
    /// </summary>
    public interface ITargetSelector
    {
        /// <summary>
        /// 将按照顺序依次选取<br/>
        /// 如:[诸武精通]:{Character_Me,Character_Me}<br/>
        /// [送你一程]:{Summon}
        /// </summary>
        public TargetEnum[] TargetEnums { get; }
    }
}
