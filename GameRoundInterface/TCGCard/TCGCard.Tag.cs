using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCGCard
{
    /// <summary>
    /// 卡片具有的属性词条
    /// 如[地区][种类]
    /// </summary>
    public interface CardTagBase
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
    /// <summary>
    /// 提供一些默认的tag
    /// 当然你也可以创建你自己的
    /// 需要搭配枚举使用
    /// </summary>
    public static class CardTags
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
            Abyss = 0,
            Mondstadt = 1,
            Liyue = 2,
            Inazuma = 3,
            Sumeru = 4,
            Fontaine = 5,
            Natlan = 6
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
