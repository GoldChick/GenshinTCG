﻿using System.Diagnostics;

namespace TCGBase
{
    public static class Tags
    {
        public static readonly string NATURE = "minecraft:nature";
        public static readonly string SUMMON = "minecraft:summon";
        #region Element & Reaction
        public static class ElementTags
        {
            //NOTE:这里是按照七国顺序排列，但是实际上的结算一般按照 冰水火雷岩草风
            public static readonly string TRIVAL = "minecraft:trival";
            public static readonly string ANEMO = "minecraft:anemo";
            public static readonly string GEO = "minecraft:geo";
            public static readonly string ELECTRO = "minecraft:electro";
            public static readonly string DENDRO = "minecraft:dendro";
            public static readonly string HYDRO = "minecraft:hydro";
            /// <summary>
            /// 火
            /// </summary>
            public static readonly string PYRO = "minecraft:pyro";
            /// <summary>
            /// 冰
            /// </summary>
            public static readonly string CRYO = "minecraft:cryo";
        }
        public static class ReactionTags
        {
            public static readonly string FROZEN = "minecraft:frozen";
            public static readonly string MELT = "minecraft:melt";
            public static readonly string SUPERCONDUCT = "minecraft:superconduct";
            /// <summary>
            /// 蒸发
            /// </summary>
            public static readonly string VAPORIZE = "minecraft:vaporize ";
            /// <summary>
            /// 感电
            /// </summary>
            public static readonly string ELECTRO_CHARGED = "minecraft:electro_charged ";
            public static readonly string OVERLOADED = "minecraft:overloaded ";

            public static readonly string CRYSTALLIZE = "minecraft:crystallize";
            public static readonly string BLOOM = "minecraft:bloom";
            public static readonly string BURNING = "minecraft:burning";
            /// <summary>
            /// 激化
            /// </summary>
            public static readonly string CATALYZE = "minecraft:catalyze";
            /// <summary>
            /// 扩散
            /// </summary>
            public static readonly string SWIRL = "minecraft:swirl ";
        }
        #endregion
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

            /// <summary>
            /// 我方行动开始前，如[天狐霆雷]
            /// </summary>
            public static readonly string ROUND_ME_START = "minecraft:round_me_start";
            #region 某个Player Action前，用于减费等
            public static readonly string BEFORE_SWITCH = "minecraft:before_switch";
            //并没有实际作用，只是占位符
            public static readonly string BEFORE_BLEND = "minecraft:before_blend";

            public static readonly string BEFORE_USE_CARD = "minecraft:before_use_card";
            public static readonly string BEFORE_USE_SKILL = "minecraft:before_use_skill";

            //并没有实际作用，只是占位符
            public static readonly string BEFORE_PASS = "minecraft:before_pass";
            //附魔>增伤(火共鸣) 增伤>乘伤(护体岩铠)
            public static readonly string ELEMENT_ENCHANT = "minecraft:element_enchant";
            public static readonly string DAMAGE_ADD = "minecraft:damage_add";
            public static readonly string HURT_ADD = "minecraft:hurt_add";
            public static readonly string DAMAGE_MUL = "minecraft:damage_mul";
            public static readonly string HURT_MUL = "minecraft:hurt_mul";

            #endregion

            #region 某个Player Action结算后，用于减少effect次数、触发其他效果等
            public static readonly string AFTER_SWITCH = "minecraft:after_switch";
            public static readonly string AFTER_BLEND = "minecraft:after_blend";

            public static readonly string AFTER_USE_CARD = "minecraft:after_use_card";
            public static readonly string AFTER_USE_SKILL = "minecraft:after_use_skill";

            public static readonly string AFTER_PASS = "minecraft:after_pass";

            /// <summary>
            /// 仅用于触发effect，而且仅在其他都不触发的时候触发
            /// </summary>
            public static readonly string AFTER_ANY_ACTION = "minecraft:after_any_action";
            //受到伤害后
            public static readonly string AFTER_HURT = "minecraft:hurt";
            #endregion
            #region
            /// <summary>
            /// 击倒预处理，在此可以判定出一些“免于被击倒”之类的状态
            /// </summary>
            public static readonly string PRE_DIE = "minecraft:pre_die";
            /// <summary>
            /// 击倒处理，预处理后生命值为0才触发。
            /// </summary>
            public static readonly string DIE = "minecraft:die";
            #endregion
            /// <summary>
            /// 将actiontype转化成对应的tag，可以用来作为sender触发一些effect
            /// (注：若before，此时仍然是可撤回阶段)
            /// <br/>
            /// before_sendertag:
            /// <br/>
            /// 若带有Variable为<see cref="DiceCostVariable"/>则说明为减费
            /// 若带有Variable为<see cref="CanActionVariable"/>则说明为能否行动
            /// 若不带有任何Variable，说明只是通知一次xx行动要开始了
            /// <br/>
            /// after_sendertag:
            /// <br/>
            /// 若带有Variable为<see cref="FastActionVariable"/>则说明为是否快速行动
            /// </summary>
            public static string ActionTypeToSenderTag(ActionType type, bool before = false) => type switch
            {
                //ActionType.ReplaceSupport
                ActionType.Switch or ActionType.SwitchForced => before ? BEFORE_SWITCH : AFTER_SWITCH,
                ActionType.UseSKill => before ? BEFORE_USE_SKILL : AFTER_USE_SKILL,
                ActionType.UseCard => before ? BEFORE_USE_CARD : AFTER_USE_CARD,
                ActionType.Blend => before ? BEFORE_BLEND : AFTER_BLEND,
                ActionType.Pass => before ? BEFORE_PASS : AFTER_PASS,
                //TODO: when wrong
                _ => throw new Exception("Tags.ActionTypeToSenderTag():传入了未知的ActionType!")
            };
        }
        public static class VariableTags
        {
            public static readonly string DICE_COST = "minecraft:dice_cost";
            public static readonly string DICE_ROLLING = "minecraft:dice_rolling";

            public static readonly string DAMAGE = "minecraft:damage";
            public static readonly string CAN_ACTION = "minecraft:can_action";
            public static readonly string FAST_ACTION = "minecraft:fast_action";
            public static readonly string MULTI_DAMAGE = "minecraft:multi_damage";
        }
    }
}
