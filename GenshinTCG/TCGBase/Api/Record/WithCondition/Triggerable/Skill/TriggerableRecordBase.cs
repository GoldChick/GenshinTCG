using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum TriggerableType
    {
        Custom,

        Skill,
        Card,
        Prepare,
        //↓下为预设，转化为TriggerableRecordWithAction↓
        RoundStart,
        RoundDuring,
        RoundOver,
        RoundStep,
        AfterUseSkill,
        AfterUseCard,
        AfterSwitch,
        AfterOperation,
        AfterHurt,
        AfterElement,
        AfterHealed,
        OnCharacterOn,
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
            return new Triggerable(Type switch
            {
                TriggerableType.Card => SenderTagInner.UseCard.ToString(),
                _ => Type.ToString()
            }, GetHandler);
        }
        protected EventPersistentHandler GetHandler(AbstractTriggerable triggerable)
        {
            return (me, p, s, v) =>
            {
                if (When.TrueForAll(condition => condition.Valid(me, p, s, v)))
                {
                    Get(triggerable, me, p, s, v);
                }
            };
        }
        protected virtual void Get(AbstractTriggerable triggerable, PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            foreach (var ac in Action)
            {
                ac.GetHandler(triggerable)?.Invoke(me, p, s, v);
            }
        }
    }
    public record TriggerableRecordBaseImplement : TriggerableRecordBase
    {
        public TriggerableRecordBaseImplement(TriggerableType type, List<ActionRecordBase>? action = null, List<ConditionRecordBase>? when = null) : base(type, action, when)
        {
        }
    }
}
