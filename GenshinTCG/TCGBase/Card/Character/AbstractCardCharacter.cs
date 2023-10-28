using TCGBase;

namespace TCGBase
{
    public enum CharacterCategory
    {
        HUMAN,
        MOB
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
        /// <summary>
        /// 叫至冬国，实际上是愚人众?
        /// TODO:改一下名字
        /// </summary>
        SNEZHNAYA,
        /// <summary>
        /// 丘丘人也算个国家算了
        /// </summary>
        QQ
    }
    public abstract class AbstractCardCharacter : AbstractCardServer
    {
        public virtual int MaxHP { get => 10; }
        public virtual int MaxMP { get => 2; }
        /// <summary>
        /// @Nullable
        /// TODO: check it?
        /// </summary>
        public virtual AbstractCardPersistentEffect? DefaultEffect { get => null; }
        /// <summary>
        ///  @NonNull 角色的所有技能
        /// </summary>
        public abstract AbstractCardSkill[] Skills { get; }
        /// <summary>
        /// 主元素，用于调和和携带共鸣牌的判定等
        /// </summary>
        public abstract ElementCategory CharacterElement { get; }
        /// <summary>
        /// 角色卡的武器类型
        /// </summary>
        public abstract WeaponCategory WeaponCategory { get; }
        /// <summary>
        /// 角色卡的户口
        /// </summary>
        public abstract CharacterRegion CharacterRegion { get; }

        /// <summary>
        /// 角色卡的(生物)种类，默认为HUMAN人类
        /// </summary>
        public virtual CharacterCategory CharacterCategory { get => CharacterCategory.HUMAN; }
    }
}
