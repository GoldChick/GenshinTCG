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
        Dice,
        /// <summary>
        /// 参数: dmgA condition dmgB<br/>
        /// eg:  "dodamageaorb,pyro-3,count=3,pyro-5" 迪卢克e
        /// </summary>
        DoDamageAorB,
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
    public record class ActionRecordBaseWithTarget : ActionRecordBaseWithTeam
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CharacterTargetRecord Target { get; }
        public ActionRecordBaseWithTarget(TriggerType actionType, DamageTargetTeam team, CharacterTargetRecord? target = null) : base(actionType, team)
        {
            Target = target ?? new();
        }
    }
}
