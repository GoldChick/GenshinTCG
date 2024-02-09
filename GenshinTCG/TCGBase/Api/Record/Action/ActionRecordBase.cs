using System.Text.Json.Serialization;

namespace TCGBase
{
    /// <summary>
    /// 不同的参数之间用，分隔
    /// </summary>
    public enum TriggerType
    {
        MP,
        Damage,
        DamageAorB,
        Effect,
        Heal,
        Dice,
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
        public CharacterTargetRecord Target { get; }
        public ActionRecordBaseWithTarget(TriggerType actionType, DamageTargetTeam team, CharacterTargetRecord? target = null) : base(actionType, team)
        {
            Target = target ?? new();
        }
    }
}
