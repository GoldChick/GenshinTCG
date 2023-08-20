using System.Diagnostics;

namespace TCGBase
{
    public static class Tags
    {
        public static readonly string NATURE = "minecraft:nature";
        public static readonly string SUMMON = "minecraft:summon";
        public static class ElementTags
        {
            public static readonly string TRIVAL = "minecraft:trival";
            public static readonly string ANEMO = "minecraft:anemo";
            public static readonly string GEO = "minecraft:geo";
            public static readonly string ELECTRO = "minecraft:electro";
            public static readonly string DENDRO = "minecraft:dendro";
            public static readonly string HYDRO = "minecraft:hydro";
            public static readonly string PYRO = "minecraft:pyro";
            public static readonly string CRYO = "minecraft:cryo";
        }
        public static class CardTags
        {
            public static class AssistTags
            {
                public static readonly string PLACE = "minecraft:place";
                public static readonly string PARTNER = "minecraft:partner";
                public static readonly string ITEM = "minecraft:item";
            }
            public static class EquipmentTags
            {
                public static readonly string WEAPON = "minecraft:weapon";
                public static readonly string ARTIFACT = "minecraft:artifact";
            }
            public static class EventTags
            {
                public static readonly string RESONANCE = "minecraft:resonance";
                public static readonly string FOOD = "minecraft:food";
            }
            public static class CharacterTags
            {
                public static readonly string HUMAN = "minecraft:human";
                public static readonly string MOB = "minecraft:mob";
            }
            public static class RegionTags
            {
                public static readonly string ABYSS = "minecraft:abyss";
                public static readonly string MONDSTADT = "minecraft:mondstadt";
                public static readonly string LIYUE = "minecraft:liyue";
                public static readonly string INAZUMA = "minecraft:inazuma";
                public static readonly string SUMERU = "minecraft:sumeru";
                public static readonly string FONTAINE = "minecraft:fontaine";
                public static readonly string NATLAN = "minecraft:natlan";
                public static readonly string SNEZHNAYA = "minecraft:snezhnaya ";
            }
            public static class WeaponTags
            {
                public static readonly string OTHER = "minecraft:other";
                public static readonly string SWORD = "minecraft:sword";
                public static readonly string CLAYMORE = "minecraft:claymore";
                public static readonly string LONGWEAPON = "minecraft:longweapon";
                public static readonly string CATALYST = "minecraft:catalyst";
                public static readonly string BOW = "minecraft:bow";
            }
        }
        public static class SkillTags
        {
            public static readonly string PASSIVE = "minecraft:passive";
            public static readonly string NORMAL_ATTACK = "minecraft:normal_attack";
            public static readonly string E = "minecraft:e";
            public static readonly string Q = "minecraft:q";

            public static readonly string HEAVY_ATTACK = "minecraft:heavy_attack";
            public static readonly string DOWN_ATTACK = "minecraft:down_attack";
        }
        public static class SenderTags
        {
            public static readonly string GAME_STAGE = "minecraft:game_stage";

            /// <summary>
            /// 投掷阶段
            /// </summary>
            public static readonly string ROLLING_START = "minecraft:roling_start";
            /// <summary>
            /// 行动阶段开始时
            /// </summary>
            public static readonly string ROUND_START = "minecraft:round_start";
            /// <summary>
            /// 结束阶段
            /// </summary>
            public static readonly string ROUND_OVER = "minecraft:round_over";
            #region player action
            public static readonly string SWITCH = "minecraft:switch";
            public static readonly string BLEND = "minecraft:blend";

            public static readonly string USE_CARD = "minecraft:use_card";
            public static readonly string USE_SKILL = "minecraft:use_skill";

            public static readonly string PASS = "minecraft:pass";
            #endregion
            public static readonly string HURT = "minecraft:hurt";

            //附魔>增伤(火共鸣) 增伤>乘伤(护体岩铠)
            public static readonly string ELEMENT_ENCHANT = "minecraft:element_enchant";
            public static readonly string DAMAGE_ADD = "minecraft:damage_add";
            public static readonly string DAMAGE_MUL = "minecraft:damage_mul";

            public static string ActionTypeToSenderTag(ActionType type) => type switch
            {
                //ActionType.ReplaceSupport
                ActionType.Switch => SWITCH,
                ActionType.UseSKill => USE_SKILL,
                ActionType.UseCard => USE_CARD,
                ActionType.Blend => BLEND,
                ActionType.Pass => PASS,
                //TODO: when wrong
                _ => throw new Exception("Tags.ActionTypeToSenderTag():传入了未知的ActionType!")
            };
        }
        public static class VariableTags
        {
            public static readonly string DICE_COST = "minecraft:dice_cost";
            public static readonly string DICE_ROLLING = "minecraft:dice_rolling";

            public static readonly string DAMAGE = "minecraft:damage";
            public static readonly string MULTI_DAMAGE = "minecraft:multi_damage";
        }
    }
}
