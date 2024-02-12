using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum ConditionType
    {
        //↓下为没有参数↓
        Alive,
        CurrCharacter,
        //↓下为单int↓
        HurtMoreThan,
        HurtEquals,
        //↓下为单string↓
        HasEffect,
        HasEffectWithTag,
        HasTag,
        ///↓下为双int↓
        //Index用来表示第几个技能(若用于角色)
        CounterMoreThan,
        CounterEquals,

        //分界线，上为刚需Persistent，下为对于Damage

        //↓下为没有参数↓
        Direct,
        Summon,
        Skill,
        //↓下为单string↓
        Element,
        Reaction,
        SkillType,
        Related,
        //↓下为单int↓
        DamageMoreThan,
        DamageEquals,
        //↓下为双int↓
    }
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
        public virtual Func<PlayerTeam, AbstractSender, AbstractVariable, bool> GetPredicate()
        {
            return Type switch
            {
                ConditionType.Direct => (me, s, v) => v is DamageVariable dv && dv.Direct == DamageSource.Direct,
                //ConditionType.Summon => (me, s, v) => s is PreHurtSender phs && phs.RootSource,
                //TODO:如何判断来源Persistent
                ConditionType.Skill => (me, s, v) => s is PreHurtSender phs && phs.RootSource is ISkillable,
                _ => throw new NotImplementedException($"Unknown SB Predicate In Type: {Type}")
            };
        }
        public virtual Func<PlayerTeam, Persistent, bool> GetPersistentPredicate()
        {
            return Type switch
            {
                ConditionType.Alive => (me, p) => p is Character c && c.Alive,
                ConditionType.CurrCharacter => (me, p) => p.PersistentRegion == me.CurrCharacter,
                _ => throw new NotImplementedException($"Unknown Persistent Predicate In Type: {Type}")
            };
        }
    }
}
