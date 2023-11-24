namespace TCGBase
{
    public enum SkillCategory
    {
        A,
        E,
        Q,
        /// <summary>
        /// 不应当直接使用此Category，请考虑继承<see cref="AbstractCardSkillPassive"/>
        /// </summary>
        P
    }
    public abstract class AbstractCardSkill : IDamageSource
    {
        /// <summary>
        /// 使用后是否加充能，默认加
        /// </summary>
        public virtual bool GiveMP { get => true; }
        public abstract int[] Costs { get; }
        /// <summary>
        /// 对于无色骰，是否需要消耗同色，默认为否，即杂色
        /// </summary>
        public virtual bool CostSame { get => false; }
        public abstract SkillCategory Category { get; }
        /// <summary>
        /// 使用后发生什么<br/>
        /// targetargs是可能的自定义Additionaltargetargs(需要自己维护)<br/><br/>
        /// <b>对于被动技能targetargs[0]表示sender.teamid，character表示被动技能的主人，并且没有additionaltargetargs</b>
        /// </summary>
        public abstract void AfterUseAction(PlayerTeam me, Character c, int[] targetArgs);
        public DamageSource DamageSource => DamageSource.Character;
    }
}