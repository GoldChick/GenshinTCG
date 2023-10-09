using TCGGame;

namespace TCGCard
{
    public abstract class AbstractCardSkill : AbstractCardServer, IUsable<AbstractTeam>
    {
        /// <summary>
        /// 是否是[准备行动]<br/>
        /// [准备行动]可以触发[增伤]，但无法触发[使用技能后]的各种效果<br/>
        /// 默认为false
        /// </summary>
        public virtual bool Prepare { get=>false; }
        public abstract int[] Costs { get; }
        public abstract bool CostSame { get; }
        public abstract void AfterUseAction(AbstractTeam me, int[]? targetArgs = null);
    }
}