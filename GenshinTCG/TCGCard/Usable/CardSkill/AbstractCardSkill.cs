using TCGBase;
using TCGGame;

namespace TCGCard
{
    public enum SkillCategory
    {
        A,
        E,
        Q,
        /// <summary>
        /// 不应当直接使用此Category，请考虑继承<see cref="AbstractPassiveSkill"/>
        /// </summary>
        P
    }
    public abstract class AbstractCardSkill : AbstractCardServer, IDamageSource
    {
        /// <summary>
        /// 是否是[准备行动]<br/>
        /// [准备行动]可以触发[增伤]，但无法触发[使用技能后]的各种效果<br/>
        /// 默认为false
        /// </summary>
        public virtual bool Prepare { get => false; }
        /// <summary>
        /// 使用后是否加充能，默认加
        /// </summary>
        public virtual bool GiveMP { get => true; }
        /// <summary>
        /// 对于无色骰，是否需要消耗同色，默认为否，即杂色
        /// </summary>
        public virtual bool CostSame { get => false; }
        public abstract SkillCategory Category { get; }
        public abstract int[] Costs { get; }
        /// <summary>
        /// 使用后发生什么<br/>
        /// targetargs是可能的自定义Additionaltargetargs(需要自己维护)<br/><br/>
        /// <b>对于被动技能targetargs[0]表示teamid，并且没有additionaltargetargs</b>
        /// </summary>
        public abstract void AfterUseAction(PlayerTeam me, Character c, int[]? targetArgs = null);
        public override sealed string[] SpecialTags => new string[] { Category.ToString() };
        public DamageSource DamageSource => DamageSource.Character;
    }
}