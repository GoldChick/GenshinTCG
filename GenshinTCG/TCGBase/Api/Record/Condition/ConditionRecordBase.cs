﻿using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum ConditionType
    {
        /// <summary>
        /// 复合条件，实现[与]
        /// </summary>
        Compound,
        //↓下为没有参数↓
        Alive,
        CurrCharacter,
        SimpleFood,//<预设>"普通食物"，要求角色[活着]，并且没有[饱腹]状态
        //↓下为单string↓
        HasEffect,
        HasEffectWithTag,
        HasTag,
        Name,
        SimpleTalent,//<预设>"普通天赋"，要求角色是[出战]，[不能有限制行动状态]，[活着]，并且[符合指定nameaid]
        SimpleWeapon,//<预设>"普通武器"，要求[活着]，并且[具有指定weapon tag]
        //↓下为int↓
        HP,
        MP,
        HPLost,
        MPLost,
        DataCount,
        Counter,//Index用来表示第几个技能(若用于角色，并且信息不带有技能)；如果信息带有技能，则自动查找对应的index
        Region,//p的persistentregion

        //分界线，上为刚需Persistent，下为对于Damage

        //↓下为没有参数↓
        Direct,//要求[伤害/元素]是直接伤害
        SourceSummon,//要求sender提供的persistent为召唤物
        SourceMe,//要求sender的id为所在team的id
        SourceThis,//要求来源sender的id为所在team的id；如果是角色状态，进一步要求sender提供的persistent为所在角色；否则要求提供的persistent==自身
        TargetMe,//要求受到[伤害]的targetTeam id为所在team的id
        TargetThis,//要求受到[伤害]的targetTeam id为所在team的id；如果是角色状态，进一步要求受到伤害的index为所在角色的index

        //↓下为单string↓
        Element,//造成了[指定元素]，不指定元素则表示为[七元素]即可(不一定是伤害！)
        ElementRelated,//是伤害，并且造成了[指定元素相关反应]
        ElementReaction,//造成了[指定反应]，不指定反应则表示为[任意反应]即可(不一定是伤害！)
        SkillType,//要求[伤害]或[费用]来源于[指定种类技能]，不指定AEQ则表示为[技能]即可
        ThisCharacterCause,//<预设>"本角色造成指定种类伤害"，要求[来源本(状态附属的)角色]，[技能伤害]，[直接伤害]，不指定AEQ则表示为[技能]即可
        OurCharacterCause,//<预设>"我方角色造成指定种类伤害"，要求[来源我方]，[技能伤害]，[直接伤害]，不指定AEQ则表示为[技能]即可
        //↓下为int↓
        Damage,

        //分界线，上为对于Damage，下为对于Dice

        //分界线，上为对于Dice，下为对于Target
        AnyTarget,//target.any()
        AnyTargetWithSameIndex,//target.any() && target.all(t=>t.persistentregion==p.persistentregion)
        CanBeAppliedFrom,//p或者p附属的角色，能够被所有target的行动牌CardBase作为使用对象
    }
    public record class ConditionRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ConditionType Type { get; }
        //默认为false，表示需要满足条件；当为true时，表示需要不满足条件
        public bool Not { get; }
        /// <summary>
        /// 如果本条件不满足，Or满足也可以捏
        /// </summary>
        public ConditionRecordBase? Or { get; }
        public ConditionRecordBase(ConditionType type, bool not, ConditionRecordBase? or)
        {
            Type = type;
            Not = not;
            Or = or;
        }
        public bool Valid(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v) => (Not ^ GetPredicate(me, p, s, v)) | (Or?.Valid(me, p, s, v) ?? false);
        protected virtual bool GetPredicate(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            return Type switch
            {
                ConditionType.Direct => v is AbstractAmountVariable aav && aav.Direct == DamageSource.Direct,
                ConditionType.SourceSummon => s is IPeristentSupplier ips && ips.Persistent.CardBase.CardType == CardType.Summon,
                ConditionType.SourceMe => s.TeamID == me.TeamIndex,
                ConditionType.SourceThis => s.TeamID == me.TeamIndex && s is IPeristentSupplier ips && (ips.Persistent is Character c ? c.PersistentRegion == p.PersistentRegion : ips.Persistent == p),
                ConditionType.TargetMe => v is AbstractAmountVariable aav && aav.TargetTeam == me.TeamIndex,
                ConditionType.TargetThis => v is AbstractAmountVariable aav && aav.TargetTeam == me.TeamIndex && p != null && (me.Characters.ElementAtOrDefault(p.PersistentRegion) is null || p.PersistentRegion == aav.TargetIndex),

                ConditionType.Alive => p is Character c && c.Alive,
                ConditionType.CurrCharacter => p?.PersistentRegion == me.CurrCharacter,
                ConditionType.SimpleFood => p is Character c && c.Alive && !c.Effects.Contains("minecraft:effect_full"),
                _ => throw new NotImplementedException($"Unknown Predicate In Type: {Type}")
            };
        }
    }
    public record class ConditionRecordBaseImplement : ConditionRecordBase
    {
        public ConditionRecordBaseImplement(ConditionType type, bool not, ConditionRecordBase? or = null) : base(type, not, or)
        {
        }
    }
}
