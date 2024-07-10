using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum TriggerType
    {
        Lua,

        Damage,//造成特殊效果为with的damage
        SampleEffect,//从给出的池子里抽取一定数量effect
        EatDice,//收集未使用的元素骰/吐出已收集的元素骰,可设置最大数量、是否允许同色
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
        public virtual EventPersistentHandler? GetHandler(AbstractTriggerable triggerable)
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
}
