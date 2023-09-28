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
    /// </summary>
    public interface ISummon : IPersistent
    {
        /// <summary>
        /// 4 decoration only<br/>
        /// may got deprecated
        /// </summary>
        //public SummonCategory Category { get; }

        ///<summary>
        ///起初的使用次数，比如[杀生樱]
        /// </summary>
        public int InitialUseTimes { get; }
    }
}
