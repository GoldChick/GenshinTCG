namespace TCGBase
{
    public enum SkillCategory
    {
        /// <summary>
        /// 作为伤害来源时，表明是一个[非(主动)技能]伤害
        /// </summary>
        P,
        A,
        E,
        Q,
    }
    public abstract class AbstractCardSkill : IDamageSource
    {
        //by the way,我不认为被隐藏的非[准备]非[被动]技能有存在的必要
        /// <summary>
        /// 是否为隐藏，隐藏技能无法触发[使用技能后]<br/>
        /// [准备技能]和[被动技能]默认不触发
        /// </summary>
        public virtual bool Hidden => true;
        public abstract CostInit DamageCost { get; }
        public abstract SkillCategory DamageSkillCategory { get; }
        public abstract void AfterUseAction(AbstractTeam me, Character c);
    }
}