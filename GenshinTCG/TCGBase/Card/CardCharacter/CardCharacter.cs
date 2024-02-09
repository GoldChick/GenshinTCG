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
        Abyss,
        Mondstadt,
        Liyue,
        Inazuma,
        Sumeru,
        Fontaine,
        Natlan,
        Fatui,
        /// <summary>
        /// 丘丘人也算个国家算了
        /// </summary>
        QQ
    }
    public class CardCharacter : AbstractCardBase
    {
        public int MaxHP { get; }
        public int MaxMP { get; }
        /// <summary>
        /// 主元素，用于调和和携带共鸣牌的判定等，从Tag中获取
        /// </summary>
        public ElementCategory CharacterElement { get; init; }
        internal CardCharacter(CardRecordCharacter record) : base(record)
        {
            MaxHP = record.MaxHP;
            MaxMP = record.MaxMP;
            string? ele = Tags.Find(t => Enum.TryParse(t, true, out ElementCategory _));
            CharacterElement = ele == null ? ElementCategory.Trival : Enum.Parse<ElementCategory>(ele, true);
        }
    }
}
