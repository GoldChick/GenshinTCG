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
        public List<TargetRecord> WhenWith { get; }
        public ActionRecordBase(TriggerType type, List<TargetRecord>? whenwith)
        {
            Type = type;
            WhenWith = whenwith ?? new();
        }
        public EventPersistentHandler? GetHandler(AbstractTriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                if (WhenWith.All(t => t.GetTargets(me, p, s, v, out _).Any()))
                {
                    DoAction(triggerable, me, p, s, v);
                }
            };
        }
        protected virtual void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v) => throw new NotImplementedException($"No Action In Type: {Type}");
    }
    public record class ActionRecordBaseWithTeam : ActionRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DamageTargetTeam Team { get; }
        public ActionRecordBaseWithTeam(TriggerType actionType, DamageTargetTeam team, List<TargetRecord>? whenwith) : base(actionType, whenwith)
        {
            Team = team;
        }
    }
    public record class ActionRecordBaseWithTarget : ActionRecordBase
    {
        public TargetRecord Target { get; }
        public ActionRecordBaseWithTarget(TriggerType type, TargetRecord? target = null, List<TargetRecord>? whenwith = null) : base(type, whenwith)
        {
            Target = target ?? new();
        }
        protected override void DoAction(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            switch (Type)
            {
                case TriggerType.Switch:
                    var ps = Target.GetTargets(me, p, s, v, out var team);
                    if (ps.Any())
                    {
                        team.TrySwitchToIndex(ps[0].PersistentRegion);
                    }
                    break;
                default:
                    base.DoAction(triggerable, me, p, s, v);
                    break;
            }
        }
    }
}
