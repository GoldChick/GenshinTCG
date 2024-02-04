using System.Text.Json.Serialization;

namespace TCGBase
{
    /// <summary>
    /// 不同的参数之间用，分隔
    /// </summary>
    public enum TriggerType
    {
        MP,
        DoDamage,
        Effect,
        Heal,
        /// <summary>
        /// 参数: dmgA condition dmgB<br/>
        /// eg:  "dodamageaorb,pyro-3,count=3,pyro-5" 迪卢克e
        /// </summary>
        DoDamageAorB,
        /// <summary>
        /// 参数: dmg effectA (effectB ...)
        /// </summary>
        DoDamageWithEffect,
    }
    public enum CharacterIndexType
    {
        /// <summary>
        /// 变身成出战状态，或者是队伍中的从出战向右的角色
        /// </summary>
        Team,
        /// <summary>
        /// 一方附属该状态的角色，若无，则为出战角色
        /// </summary>
        CurrCharacter,
        NotCurrCharacter,
        /// <summary>
        /// 一方所有角色
        /// </summary>
        AllCharacter,
        /// <summary>
        /// 一方指定角色; 参数:Character.NameID
        /// </summary>
        CertainCharacter,
        /// <summary>
        /// 指定角色之外的一方角色; 参数:Character.NameID
        /// </summary>
        NotCertainCharacter,
    }
    public record class ActionRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TriggerType ActionType { get; }

        public ActionRecordBase(TriggerType actionType)
        {
            ActionType = actionType;
        }
        public virtual EventPersistentHandler? GetHandler(ITriggerable triggerable) => null;
    }
    public record class ActionRecordBaseWithTeam : ActionRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DamageTargetTeam Team { get; }
        public ActionRecordBaseWithTeam(TriggerType actionType, DamageTargetTeam team) : base(actionType)
        {
            Team = team;
        }
    }
    public record class ActionRecordBaseWithTarget : ActionRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CharacterTargetRecord Target { get; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DamageTargetTeam Team { get; }
        public ActionRecordBaseWithTarget(TriggerType actionType, DamageTargetTeam team, CharacterTargetRecord? target = null) : base(actionType)
        {
            Target = target ?? new();
            Team = team;
        }
    }
}
