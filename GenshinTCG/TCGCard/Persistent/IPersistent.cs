using TCGBase;
using TCGGame;

namespace TCGCard
{
    public interface IPersistent : ICardServer
    {
        /// <summary>
        /// 次数是否可以堆叠，如果不可堆叠，将无法通过各种额外方式增加次数
        /// </summary>
        public bool Stackable { get; }
        /// <summary>
        /// 可用次数为0时是否立即删除
        /// </summary>
        public bool DeleteWhenUsedUp { get; }
        /// <summary>
        /// 最大使用次数，需要Stackable为true才有意义
        /// </summary>
        public int MaxUseTimes { get; }
        /// <summary>
        /// string: SenderTag names<br/>
        /// team: team me<br/>
        /// persistent: this buff<br/>
        /// 在#TODO#中提供一些预设，如回合开始刷新次数，回合结束清除等
        /// </summary>
        public Dictionary<string, IPersistentTrigger> TriggerDic { get; }

    }
}
