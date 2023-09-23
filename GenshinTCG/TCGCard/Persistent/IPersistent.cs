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
        /// 结算或预结算一次效果
        /// 
        /// 注意：加费、减费的效果和增伤、减伤的效果分开写
        /// </summary>
        /// <param name="persitent">当前触发效果的persistent对应的object,用来减少、增加次数</param>
        /// <param name="sender">信息的发送者,如打出的[牌],使用的[技能]</param>
        /// <param name="variable">可以被改写的东西,如[消耗的骰子们],[伤害] <b>(不应改变类型)</b></param>
        /// <returns>是否弃置</returns>
        public void EffectTrigger(AbstractTeam me, AbstractTeam enemy,AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable);
        
    }
}
