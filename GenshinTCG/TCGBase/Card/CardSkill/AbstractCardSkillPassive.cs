namespace TCGBase
{
    public abstract class AbstractCardSkillPassive : AbstractCardSkill
    {
        public override sealed SkillCategory DamageSkillCategory => SkillCategory.P;
        public override sealed CostInit DamageCost => new();
        /// <summary>
        /// 被动技能不能触发[使用技能后]
        /// </summary>
        public override sealed bool Hidden => false;
        /// <summary>
        /// 为true时，[此技能]只会在开场时触发一次<br/>
        /// 为false时，复活也会重新触发<br/>
        /// </summary>
        public abstract bool TriggerOnce { get; }
        /// <summary>
        /// 什么时候触发,绑定在Character上<br/>
        /// 注:由于Dictionary Key不可重复，所以只能委屈一下sendertag和passiveskill一一对应
        /// </summary>
        public abstract string TriggerSenderTag { get; }
        /// <summary>
        /// 一般用了不会发生什么
        /// </summary>
        public override void AfterUseAction(AbstractTeam me, Character c)
        {
        }
        internal void AfterTriggerAction(AbstractTeam me, AbstractPersistent p, AbstractSender s, AbstractVariable? v)
        {
            if (p is Character c)
            {
                AfterUseAction(me, c);
            }
        }
    }
}