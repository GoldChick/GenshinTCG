namespace TCGBase
{
    public abstract class AbstractPassiveSkill : AbstractCardSkill, ICardPersistent
    {
        public override sealed SkillCategory Category => SkillCategory.P;
        public override sealed int[] Costs => Array.Empty<int>();
        public override sealed bool CostSame => false;
        public override sealed bool GiveMP => false;
        /// <summary>
        /// 为true时，[此技能]只会在开场时触发一次<br/>
        /// 为false时，复活也会重新触发<br/>
        /// <br/>
        /// 触发后，给携带该技能的角色上一个effect
        /// </summary>
        public abstract bool TriggerOnce { get; }

        public string Namespace => GetType().Namespace??"minecraft";

        public string NameID => GetType().Name;

        public virtual int InitialUseTimes => MaxUseTimes;

        public abstract int MaxUseTimes { get; }

        public abstract PersistentTriggerDictionary TriggerDic { get; }

        public int Variant { get; }

        public virtual bool CustomDesperated => false;

        /// <summary>
        /// 一般不应该更新被动技能
        /// </summary>
        public void Update<T>(Persistent<T> persistent) where T : ICardPersistent
        {
        }
    }
}