namespace TCGBase
{
    public abstract class AbstractCardSkillPassive : AbstractCardSkill, ICardPersistent
    {
        public override sealed SkillCategory Category => SkillCategory.P;
        public override sealed CostInit Cost => new();
        public override int GiveMP => 0;
        public override bool TriggerAfterUseSkill => false;
        /// <summary>
        /// 为true时，[此技能]只会在开场时触发一次<br/>
        /// 为false时，复活也会重新触发<br/>
        /// <br/>
        /// 触发后，给携带该技能的角色上一个effect
        /// </summary>
        public abstract bool TriggerOnce { get; }

        public string Namespace => GetType().Namespace ?? "minecraft";

        public string NameID => GetType().Name;

        public virtual int InitialUseTimes => MaxUseTimes;
        /// <summary>
        /// 可用次数大于等于0才会添加effect到角色身上
        /// </summary>
        public abstract int MaxUseTimes { get; }

        public virtual PersistentTriggerDictionary TriggerDic => new();

        public int Variant { get; protected set; }

        public virtual bool CustomDesperated => false;
        /// <summary>
        /// 一般用了不会发生什么
        /// </summary>
        public override void AfterUseAction(PlayerTeam me, Character c)
        {
        }
        /// <summary>
        /// 一般不应该更新被动技能
        /// </summary>
        public virtual void Update<T>(PlayerTeam me, Persistent<T> persistent) where T : ICardPersistent
        {
        }
        public virtual void OnDesperated(PlayerTeam me, int region)
        {
        }
    }
}