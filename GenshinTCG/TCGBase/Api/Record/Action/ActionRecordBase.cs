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
        Counter,
        Switch,
        Skill,
        SetData
    }
    public record class ActionRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TriggerType Type { get; }
        /// <summary>
        /// 所有record都要找到符合要求的target
        /// </summary>
        public List<TargetRecord> When { get; }
        public ActionRecordBase(TriggerType type, List<TargetRecord>? when)
        {
            Type = type;
            When = when ?? new();
        }
        public virtual EventPersistentHandler? GetHandler(AbstractTriggerable triggerable) => throw new NotImplementedException($"No Action In Type: {Type}");
    }
    public record class ActionRecordBaseWithTeam : ActionRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DamageTargetTeam Team { get; }
        public ActionRecordBaseWithTeam(TriggerType actionType, DamageTargetTeam team, List<TargetRecord>? when) : base(actionType, when)
        {
            Team = team;
        }
    }
    public record class ActionRecordBaseWithTarget : ActionRecordBase
    {
        public TargetRecord Target { get; }
        public ActionRecordBaseWithTarget(TriggerType type, TargetRecord? target = null, List<TargetRecord>? when = null) : base(type, when)
        {
            Target = target ?? new();
        }
        public override EventPersistentHandler? GetHandler(AbstractTriggerable triggerable)
        {
            return Type switch
            {
                TriggerType.Switch => (me, p, s, v) =>
                {
                    var ps = Target.GetTargets(me, p, s, out var team);
                    if (ps.Any())
                    {
                        team.TrySwitchToIndex(ps[0].PersistentRegion);
                    }
                }
                ,
                _ => base.GetHandler(triggerable),
            };
        }
    }
}
