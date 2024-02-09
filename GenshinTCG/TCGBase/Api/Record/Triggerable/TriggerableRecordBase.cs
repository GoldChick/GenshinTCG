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

        public TriggerableRecordBase(TriggerableType type)
        {
            Type = type;
        }
        public virtual AbstractCustomTriggerable GetTriggerable() => throw new NotImplementedException($"某个继承了TriggerableRecordBase的record class没有实现GetTriggerable()!Type: {Type}.");
    }
    public record TriggerableRecordWithAction : TriggerableRecordBase
    {
        public List<ActionRecordBase> Action { get; }

        public TriggerableRecordWithAction(TriggerableType type, List<ActionRecordBase> action) : base(type)
        {
            Action = action;
        }
        public override AbstractCustomTriggerable GetTriggerable() => new Triggerable(this);
    }
}
