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
        SimpleTalent,//预设普通天赋，要求角色是出战，不能有限制行动状态，或者，并且符合指定名字
        ///↓下为int↓
        HPLost,
        MPLost,
        //Index用来表示第几个技能(若用于角色)
        Counter,


        //分界线，上为刚需Persistent，下为对于Damage

        //↓下为没有参数↓
        Direct,
        Summon,
        Skill,//要求伤害来源于技能
        SourceMe,//要求sender的id为所在team的id
        TargetMe,//要求伤害的target id为所在team的id
        //↓下为单string↓
        Element,
        Reaction,
        SkillType,
        Related,
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
                ConditionType.Skill => s is HurtSourceSender hss && hss.Triggerable is ISkillable,
                ConditionType.SourceMe => s != null && s.TeamID == me.TeamIndex,
                ConditionType.TargetMe => s is HurtSourceSender hss && v is DamageVariable dv && (hss.TeamID ^ (int)dv.TargetTeam) != me.TeamIndex,

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
