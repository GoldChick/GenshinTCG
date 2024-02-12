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
        ///↓下为int↓
        HPLost,
        //Index用来表示第几个技能(若用于角色)
        Counter,

        //分界线，上为刚需Persistent，下为对于Damage

        //↓下为没有参数↓
        Direct,
        Summon,
        Skill,
        SourceMe,
        TargetMe,
        //↓下为单string↓
        Element,
        Reaction,
        SkillType,
        Related,
        //↓下为int↓
        Damage,

        //分界线，上为对于Damage，下为对于Dice
    }
    public delegate bool EventPersistentPredicate(PlayerTeam me, Persistent? p, AbstractSender? s, AbstractVariable? v);
    public record class ConditionRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ConditionType Type { get; }
        //默认为false，表示需要满足条件；当为true时，表示需要不满足条件
        public bool Not { get; }
        public ConditionRecordBase(ConditionType type, bool not)
        {
            Type = type;
            Not = not;
        }
        public bool Valid(PlayerTeam me, Persistent? p, AbstractSender? s, AbstractVariable? v) => Not ^ GetPredicate(me, p, s, v);
        protected virtual bool GetPredicate(PlayerTeam me, Persistent? p, AbstractSender? s, AbstractVariable? v)
        {
            return Type switch
            {
                ConditionType.Direct => v is DamageVariable dv && dv.Direct == DamageSource.Direct,
                ConditionType.Summon => s is HurtSourceSender hss && hss.Source.CardBase.CardType == CardType.Summon,
                ConditionType.Skill => s is HurtSourceSender hss && hss.Triggerable is ISkillable,
                ConditionType.SourceMe => s is HurtSourceSender hss && hss.TeamID == me.TeamIndex,
                ConditionType.TargetMe => s is HurtSourceSender hss && v is DamageVariable dv && (hss.TeamID ^ (int)dv.DamageTargetTeam) != me.TeamIndex,

                ConditionType.Alive => p is Character c && c.Alive,
                ConditionType.CurrCharacter => p?.PersistentRegion == me.CurrCharacter,
                _ => throw new NotImplementedException($"Unknown Predicate In Type: {Type}")
            };
        }
    }
}
