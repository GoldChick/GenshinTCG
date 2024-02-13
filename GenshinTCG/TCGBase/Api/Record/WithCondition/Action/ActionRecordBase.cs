using System.Text.Json.Serialization;

namespace TCGBase
{
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
    public record class ActionRecordBase : IWhenAnyThenAction
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TriggerType Type { get; }

        public List<List<ConditionRecordBase>> WhenAny { get; }

        public ActionRecordBase(TriggerType type, List<List<ConditionRecordBase>>? whenany)
        {
            Type = type;
            WhenAny = whenany ?? new();
        }
        public EventPersistentHandler? GetHandler(AbstractTriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                if ((this as IWhenAnyThenAction).IsConditionValid(me, p, s, v))
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
        public ActionRecordBaseWithTeam(TriggerType actionType, DamageTargetTeam team, List<List<ConditionRecordBase>>? whenany = null) : base(actionType, whenany)
        {
            Team = team;
        }
    }
    public record class ActionRecordBaseWithTarget : ActionRecordBase
    {
        public TargetRecord Target { get; }
        public ActionRecordBaseWithTarget(TriggerType type, TargetRecord? target = null, List<List<ConditionRecordBase>>? whenany = null) : base(type, whenany)
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
