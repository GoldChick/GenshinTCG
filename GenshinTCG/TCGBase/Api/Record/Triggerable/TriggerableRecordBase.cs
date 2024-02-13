using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum TriggerableType
    {
        Custom,

        Skill,
        Card,
        //↓下为预设，转化为TriggerableRecordWithAction↓
        RoundOver,
        RoundStep,
        AfterUseSkill,
        AfterUseCard,
        AfterSwitch,
        AfterHurt,
        AfterHealed,
        //↓下为预设，有设置好的类↓
        Skill_A,
        Skill_E,
    }
    public record class TriggerableRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TriggerableType Type { get; }
        public List<ActionRecordBase> Action { get; }
        public List<ConditionRecordBase> When { get; }
        public TriggerableRecordBase(TriggerableType type, List<ActionRecordBase>? action = null, List<ConditionRecordBase>? when = null)
        {
            Type = type;
            Action = action ?? new();
            When = when ?? new();
        }
        public virtual AbstractTriggerable GetTriggerable()
        {
            Triggerable t = new(SenderTag.RoundStep.ToString());
            if (Type == TriggerableType.RoundStep)
            {
                t.Action = GetHandler(t);
                return t;
            }
            else if (Type == TriggerableType.RoundOver)
            {
                t = new(SenderTag.RoundOver.ToString());
                t.Action = GetHandler(t);
                return t;
            }
            return Type switch
            {
                TriggerableType.Card => new TriggerableCard(this),
                _ => throw new NotImplementedException($"某个继承了TriggerableRecordBase的record class没有实现GetTriggerable()!Type: {Type}.")
            };
        }
        protected virtual EventPersistentHandler? GetHandler(AbstractTriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                if (When.TrueForAll(condition => condition.Valid(me, p, s, v)))
                {
                    foreach (var ac in Action)
                    {
                        ac.GetHandler(triggerable)?.Invoke(me, p, s, v);
                    }
                }
            };
        }
    }
    public record TriggerableRecordBaseImplement : TriggerableRecordBase
    {
        public TriggerableRecordBaseImplement(TriggerableType type, List<ActionRecordBase>? action = null) : base(type, action)
        {
        }
    }
}
