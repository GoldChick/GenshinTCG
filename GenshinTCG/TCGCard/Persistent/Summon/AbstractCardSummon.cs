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
    public abstract class AbstractCardSummon : AbstractCardPersistent
    {
        public virtual int InitialUseTimes { get => MaxUseTimes; }
    }
}
