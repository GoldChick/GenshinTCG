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
    public record class ActionRecordBase : IWhenThenAction
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TriggerType Type { get; }

        public List<ConditionRecordBase> When { get; }

        public ActionRecordBase(TriggerType type, List<ConditionRecordBase>? when)
        {
            Type = type;
            When = when ?? new();
        }
        public EventPersistentHandler? GetHandler(AbstractTriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                if ((this as IWhenThenAction).IsConditionValid(me, p, s, v))
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
        public TargetTeam Team { get; }
        public ActionRecordBaseWithTeam(TriggerType actionType, TargetTeam team, List<ConditionRecordBase>? when = null) : base(actionType, when)
        {
            Team = team;
        }
    }
    public record class ActionRecordBaseWithTarget : ActionRecordBase
    {
        public TargetRecord Target { get; }
        public ActionRecordBaseWithTarget(TriggerType type, TargetRecord? target = null, List<ConditionRecordBase>? when = null) : base(type, when)
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
