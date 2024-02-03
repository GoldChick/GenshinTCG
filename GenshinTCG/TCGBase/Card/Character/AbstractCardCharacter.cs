﻿namespace TCGBase
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
    public abstract class AbstractCardCharacter : AbstractCardBase
    {
        public abstract int MaxHP { get; }
        public abstract int MaxMP { get; }
        /// <summary>
        /// 主元素，用于调和和携带共鸣牌的判定等，从Tag中获取
        /// </summary>
        public ElementCategory CharacterElement { get; init; }
        public override sealed bool CustomDesperated => true;
        public override sealed int MaxUseTimes => 0;
        protected AbstractCardCharacter() : base("null")
        {
            string? ele = Tags.Find(t => Enum.TryParse(t, out ElementCategory _));
            CharacterElement = ele == null ? ElementCategory.Trival : Enum.Parse<ElementCategory>(ele);
        }
        protected private AbstractCardCharacter(CharacterCardRecord record) : base(record)
        {
            string? ele = Tags.Find(t => Enum.TryParse(t, out ElementCategory _));
            CharacterElement = ele == null ? ElementCategory.Trival : Enum.Parse<ElementCategory>(ele);
        }
    }
    public class CardCharacter : AbstractCardCharacter
    {
        public override int MaxHP { get; }
        public override int MaxMP { get; }
        internal CardCharacter(CharacterCardRecord record) : base(record)
        {
            MaxHP = record.MaxHP;
            MaxMP = record.MaxMP;
        }
    }
}
