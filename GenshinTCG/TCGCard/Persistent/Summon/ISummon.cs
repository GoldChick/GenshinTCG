namespace TCGCard
{
    public enum SummonCategory
    {
        /// <summary>
        /// 沙漏
        /// </summary>
        Trival,
        /// <summary>
        /// 沙漏
        /// </summary>
        Attack,
        /// <summary>
        /// 盾
        /// </summary>
        Defend
    }
    /// <summary>
    /// 召唤物区的召唤物<br/>
    /// [回合末]结算后若可用次数为0，立即弃置<br/>
    /// 没有可用次数(Stackable=false)在召唤物区域视为可用次数为1
    /// </summary>
    public interface ISummon : IPersistent
    {
        public SummonCategory Category { get; }
        /// <summary>
        /// 为true则可用次数为0时立即弃置<br/>
        /// 为false则可用次数为0时不弃置，坚持到回合末结算效果
        /// </summary>
        public bool MustPossitive { get; }
    }
}
