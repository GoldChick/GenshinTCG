using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum TriggerType
    {
        //int with target
        MP,//target角色增加value点mp
        Skill,//target角色使用value技能
        Prepare,//target角色使用value准备技能
        Heal,//target角色治疗value点mp
        //string
        Trigger,//从所在队伍发送，全局触发名为value的状态结算轮
        Element,//target角色附着value元素

        //with target
        Switch,//target的team切换到第一个可用角色目标
        Destroy,//弃置target中所有非角色状态；target.type若为Character(未指定)，则弃置自身

        DrawCard,//指定team抽value张满足withtag的牌
        Damage,//造成特殊效果为with的damage
        Effect,//针对target先删除remove的，再添加add的，支持 行动牌、召唤物、状态
        SampleEffect,//从给出的池子里抽取一定数量effect
        Dice,//根据gain，获得或失去dice中的内容
        EatDice,//收集未使用的元素骰/吐出已收集的元素骰,可设置最大数量、是否允许同色
        Counter,//(force去)add或set指定targer的persistent或者自身的Counter
        SetData,
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
            var ps = Target.GetTargets(me, p, s, v, out var team);
            switch (Type)
            {
                case TriggerType.Switch:
                    if (ps.ElementAtOrDefault(0) is Character c)
                    {
                        team.TrySwitchToIndex(c.PersistentRegion);
                    }
                    break;
                case TriggerType.Destroy:
                    switch (Target.Type)
                    {
                        case TargetType.Character:
                            p.Active = false;
                            break;
                        default:
                            foreach (var per in ps)
                            {
                                per.Active = false;
                            }
                            break;
                    }
                    break;
                default:
                    base.DoAction(triggerable, me, p, s, v);
                    break;
            }
        }
    }
}
