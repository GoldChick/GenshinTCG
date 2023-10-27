using TCGBase;
using TCGGame;

namespace TCGBase
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
    public abstract class AbstractCardPersistentSummon : AbstractCardPersistent,ISinglePersistentProvider<AbstractCardPersistentSummon>,IDamageSource
    {
        /// <summary>
        /// 召唤物默认为空tag<br/>
        /// TODO:没有注意过召唤物是否有tag
        /// </summary>
        public override sealed string[] SpecialTags => Array.Empty<string>();

        public AbstractCardPersistentSummon PersistentPool => this;

        public DamageSource DamageSource => DamageSource.Summon;

        /// <summary>
        /// 重复刷新召唤物的时候会如何行动，默认为取[当前次数]和[最大次数]的最大值
        /// </summary>
        public virtual void Update(Persistent<AbstractCardPersistentSummon> persistent)
        {
            persistent.AvailableTimes = int.Max(persistent.AvailableTimes, MaxUseTimes);
        }
    }
}
