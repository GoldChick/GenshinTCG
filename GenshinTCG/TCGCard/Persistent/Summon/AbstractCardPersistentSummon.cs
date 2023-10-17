using TCGGame;

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
    public abstract class AbstractCardPersistentSummon : AbstractCardPersistent
    {
        /// <summary>
        /// 召唤物默认为空tag<br/>
        /// TODO:没有注意过召唤物是否有tag
        /// </summary>
        public override sealed string[] SpecialTags => Array.Empty<string>();
        /// <summary>
        /// 召唤物产生时候的基础使用次数，默认和[最大次数]一样
        /// </summary>
        public virtual int InitialUseTimes { get => MaxUseTimes; }
        /// <summary>
        /// 重复刷新召唤物的时候会如何行动，默认为取[当前次数]和[最大次数]的最大值
        /// </summary>
        public virtual void Update(AbstractPersistent<AbstractCardPersistentSummon> persistent)
        {
            persistent.AvailableTimes = int.Max(persistent.AvailableTimes, MaxUseTimes);
        }
    }
}
