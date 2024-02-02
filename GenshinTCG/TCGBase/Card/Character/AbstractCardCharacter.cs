namespace TCGBase
{
    public enum CharacterCategory
    {
        Human,
        Mob
    }
    public enum CharacterRegion
    {
        None,
        ABYSS,
        MONDSTADT,
        LIYUE,
        INAZUMA,
        SUMERU,
        FONTAINE,
        NATLAN,
        Fatui,
        /// <summary>
        /// 丘丘人也算个国家算了
        /// </summary>
        QQ
    }
    public abstract class AbstractCardCharacter : AbstractCardPersistent
    {
        public int MaxHP { get; }
        public int MaxMP { get; }
        /// <summary>
        ///  @NonNull 角色的所有技能
        /// </summary>
        public abstract AbstractSkillTrigger[] Skills { get; }
        /// <summary>
        /// 主元素，用于调和和携带共鸣牌的判定等
        /// </summary>
        public abstract ElementCategory CharacterElement { get; }
        public override sealed bool CustomDesperated => true;
        public override sealed int MaxUseTimes => 0;
        internal AbstractCardCharacter(CharacterCardRecord record) : base(record)
        {
            MaxHP = record.MaxHP;
            MaxMP = record.MaxMP;
        }
    }
}
