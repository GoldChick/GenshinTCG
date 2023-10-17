﻿using TCGBase;

namespace TCGCard
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
        SNEZHNAYA
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

        /// <summary>
        /// 用来存放比较特殊的tag，比如[丘丘岩盔王]：[丘丘人]
        /// </summary>
        public override sealed string[] SpecialTags => new List<string>()
            {
                CharacterElement.ToString(),
                CharacterCategory.ToString(),
                CharacterRegion.ToString(),
                WeaponCategory.ToString()
            }
        .Where(s => !string.IsNullOrEmpty(s)).ToArray();
        //TODO: how to to string?
    }
}
