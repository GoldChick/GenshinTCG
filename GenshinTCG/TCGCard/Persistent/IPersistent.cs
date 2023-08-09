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
        /// 是否每回合（开始时）刷新可用次数
        /// </summary>
        public bool Update { get; }
        /// <summary>
        /// 最大使用次数，需要Stackable为true才有意义
        /// </summary>
        public int MaxUseTimes { get; }


        /// <summary>
        /// 结算或预结算一次效果
        /// </summary>
        /// <param name="sender">信息的发送者,如打出的[牌],使用的[技能]</param>
        /// <param name="variable">可以被改写的东西,如[消耗的骰子们],[伤害] <b>(不应改变类型)</b></param>
        public bool TryAct(AbstractTeam me,AbstractTeam enemy,AbstractSender sender,AbstractVariable? variable) ;

        public void AfterAct(AbstractTeam me, AbstractTeam enemy);
        /// <summary> 
        /// 获得时的效果
        /// </summary>
        public void WhenGain(AbstractTeam me, AbstractTeam enemy) ;
        /// <summary>
        /// 弃置时的效果
        /// </summary>
        public void WhenDesperated(AbstractTeam me, AbstractTeam enemy) ;


        /// <summary>
        /// 值越大，越靠后触发<br/>
        /// @desperated 角色>召唤物>支援区
        /// </summary>
        /// public int Prior { get; }
    }
}
