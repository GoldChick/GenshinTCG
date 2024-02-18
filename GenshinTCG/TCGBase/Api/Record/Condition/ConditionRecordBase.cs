using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum ConditionType
    {
        //↓下为没有参数↓
        Alive,
        CurrCharacter,
        //↓下为单string↓
        HasEffect,
        HasEffectWithTag,
        HasTag,
        Name,
        SimpleTalent,//<预设>"普通天赋"，要求角色是[出战]，[不能有限制行动状态]，[活着]，并且[符合指定nameaid]
        ///↓下为int↓
        HPLost,
        MPLost,
        //Index用来表示第几个技能(若用于角色)
        Counter,


        //分界线，上为刚需Persistent，下为对于Damage

        //↓下为没有参数↓
        Direct,
        Summon,
        SourceMe,//要求sender的id为所在team的id
        TargetMe,//要求伤害的target id为所在team的id
        TargetThis,//要求受到伤害的targetTeam id为所在team的id；如果是角色状态，进一步要求受到伤害的index为所在角色的index

        //↓下为单string↓
        Element,//xx元素
        Reaction,//xx反应
        SkillType,//要求伤害来源于[指定种类技能]，不指定AEQ则表示为[技能]即可
        OurCharacterCause,//<预设>"我方角色造成指定种类伤害"，要求[来源我方]，[技能伤害]，[直接伤害]，不指定AEQ则表示为[技能]即可
        Related,//xx元素相关反应
        //↓下为int↓
        Damage,

        //分界线，上为对于Damage，下为对于Dice

        //分界线，上为对于Dice，下为对于Target
        AnyTarget,
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
        public bool Valid(PlayerTeam me, Persistent? p, AbstractSender? s, AbstractVariable? v) => (Not ^ GetPredicate(me, p, s, v)) | (Or?.Valid(me, p, s, v) ?? false);
        protected virtual bool GetPredicate(PlayerTeam me, Persistent? p, AbstractSender? s, AbstractVariable? v)
        {
            return Type switch
            {
                ConditionType.Direct => v is DamageVariable dv && dv.Direct == DamageSource.Direct,
                ConditionType.Summon => s is HurtSourceSender hss && hss.Source.CardBase.CardType == CardType.Summon,
                ConditionType.SourceMe => s != null && s.TeamID == me.TeamIndex,
                ConditionType.TargetMe => v is DamageVariable dv && dv.TargetTeam == me.TeamIndex,
                ConditionType.TargetThis => v is DamageVariable dv && dv.TargetTeam == me.TeamIndex && p != null && (me.Characters.ElementAtOrDefault(p.PersistentRegion) is null || p.PersistentRegion == dv.TargetIndex),

                ConditionType.Alive => p is Character c && c.Alive,
                ConditionType.CurrCharacter => p?.PersistentRegion == me.CurrCharacter,
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
