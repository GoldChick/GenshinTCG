using System.Collections.Generic;
//################################################################
//Server和Client都要使用的Lib
//################################################################
namespace TCGCard
{
    /// <summary>
    /// 每个提供卡片的dll都要有唯一一个实现IUtils的类来提供此dll的信息
    /// mod的版本使用[AssemblyVersion]指定
    /// </summary>
    public interface IUtils
    {
        /// <summary>
        /// 此MOD独有的namespace，用于与其他mod作区分
        /// 不可重复，否则不能正常加载
        /// </summary>
        public string NameSpace { get; }
        /// <summary>
        /// 对于此mod的描述
        /// 随便写写就行
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// 作者的名字
        /// 推荐大家诚实一点不要冒充别人
        /// </summary>
        public string Author { get; }
        /// <summary>
        /// 依赖项的namespace（作为某些mod的子mod时使用）
        /// 具体的呈现格式为"minecraft:keqing"中的"minecraft"
        /// </summary>
        public List<string> Dependencies { get; }
    }
    public interface ICardBase
    {
        /// <summary>
        /// 并不是卡牌的名字
        /// 而是卡牌对应的id
        /// 如"minecraft:keqing"中的"keqing"
        /// </summary>
        public string NameID { get; }
    }
    /// <summary>
    /// 卡片具有的属性词条
    /// 如[地区][种类]
    /// </summary>
    public interface CardAttributeBase
    {
        /// <summary>
        /// 属性词条的种类
        /// 如[国家_璃月]中的国家
        /// </summary>
        public string NameSpace { get; }
        /// <summary>
        /// 词条名字
        /// 如[国家_璃月]中的璃月
        /// </summary>
        public string Name { get; }
    }
    public interface CardAttribute<T> : CardAttributeBase
    {

    }
    /// <summary>
    /// 提供一些默认的attribute
    /// 当然你也可以创建你自己的
    /// 需要搭配枚举使用
    /// </summary>
    public static class CardAttributes
    {
        public static readonly string CHARACTER_TYPE = "character_type";
        public static readonly string CHARACTER_REGION = "character_region";
        public static readonly string CARDASSIST_TYPE = "cardassist_type";
        public static readonly string WEAPON_TYPE = "weapon_type";

        public enum CharacterType
        {
            Human,
            Mob
        }
        public enum CharacterRegion
        {
            Abyss,
            Mondstadt,
            Liyue,
            Inazuma,
            Sumeru,
            Fontaine,
            Natlan
        }
        public enum CardAssistType
        {
            Nature,
            Weapon,
            Artifact,
            Place,
            Food,
            Event,
            Summon
        }
        public enum WeaponType
        {
            Other,
            Sword,
            BigSword,
            LongWeapon,
            Catalyst,
            Bow
        }
    }
}



