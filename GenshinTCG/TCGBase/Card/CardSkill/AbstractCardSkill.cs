namespace TCGBase
{
    public enum SkillCategory
    {
        /// <summary>
        /// 不应当直接使用此Category，请考虑继承<see cref="AbstractCardSkillPassive"/><br/>
        /// 作为伤害来源时，表明是一个非主动技能伤害
        /// </summary>
        P,
        A,
        E,
        Q,
    }
    public abstract class AbstractCardSkill : IDamageSource
    {
        /// <summary>
        /// 使用后加多少充能，默认加1<br/>
        /// 如果Cost中CostMP>0，表明需要消耗充能，则会忽略此属性，不产生充能<br/>
        /// [准备技能]和[被动技能]默认不产生充能
        /// </summary>
        public virtual int GiveMP => 1;
        /// <summary>
        /// 是否触发[使用技能后]，默认触发<br/>
        /// [准备技能]和[被动技能]默认触发
        /// </summary>
        public virtual bool TriggerAfterUseSkill => true;
        public abstract CostInit Cost { get; }
        public abstract SkillCategory Category { get; }
        public SkillCategory DamageSkillCategory => Category;
        /// <summary>
        /// 使用后发生什么<br/>
        /// targetargs是可能的自定义Additionaltargetargs(需要自己维护)<br/><br/>
        /// <b>对于被动技能targetargs[0]表示sender.teamid，character表示被动技能的主人，并且没有additionaltargetargs</b>
        /// </summary>
        public abstract void AfterUseAction(PlayerTeam me, Character c, int[] targetArgs);
    }
}