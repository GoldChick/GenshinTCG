using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum ConditionType
    {
        /// <summary>
        /// 使用lua脚本描述条件，自带me,p,s,v作为参数，由于并非即时运行，所以可能含有隐含错误。建议编写时进行模拟测试。<br/> 
        /// 返回值：需要在脚本中为名叫result的变量赋值true或false，否则默认为false<br/>
        /// 注意：lua中的索引从1开始，而C#中的索引从0开始！<br/>
        /// TODO:加入日志系统
        /// </summary>
        Lua,
        /// <summary>
        /// 复合条件，实现[与]
        /// </summary>
        Compound,
        //↓下为没有参数↓
        GameStart,//游戏开始时，用于[潜行大师]等被动触发
        SimpleFood,//<预设>"普通食物"，要求角色[活着]，并且没有[饱腹]状态
        //↓下为单string↓
        HasEffectWithTag,
        HasCard,//CardsInHand是否有指定名字的卡牌
        OperationType,//用于afteroperation，指定必须是给定的种类，未指定则任意即可
        SimpleTalent,//<预设>"普通天赋"，要求角色是[出战]，[不能有限制行动状态]，[活着]，并且[符合指定nameaid]
        SimpleWeapon,//<预设>"普通武器"，要求[活着]，并且[具有指定weapon tag]

        //↓下为没有参数↓
        SourceMe,//要求sender的id为所在team的id
        SourceThis,//要求来源sender的id为所在team的id；如果是角色状态，进一步要求sender提供的persistent为所在角色；否则要求提供的persistent==自身
        TargetMe,//要求受到[伤害]的targetTeam id为所在team的id
        TargetThis,//要求受到[伤害]的targetTeam id为所在team的id；如果是角色状态，进一步要求受到伤害的index为所在角色的index；否则要求index为出战角色

        //↓下为单string↓
        ElementRelated,//是伤害，并且造成了[指定元素相关反应]
        ThisCharacterCause,//<预设>"本角色造成指定种类伤害"，要求[来源本(状态附属的)角色]，[技能伤害]，[直接伤害]，不指定AEQ则表示为[技能]即可
        OurCharacterCause,//<预设>"我方角色造成指定种类伤害"，要求[来源我方]，[技能伤害]，[直接伤害]，不指定AEQ则表示为[技能]即可
    }
    public record class ConditionRecordBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ConditionType Type { get; }
        //默认为false，表示需要满足条件；当为true时，表示需要不满足条件
        public bool Not { get; }
        /// <summary>
        /// 如果本条件不满足，Or满足也可以捏
        /// </summary>
        public ConditionRecordBase? Or { get; }
        public ConditionRecordBase(ConditionType type, bool not, ConditionRecordBase? or = null)
        {
            Type = type;
            Not = not;
            Or = or;
        }
        public bool Valid(PlayerTeam me, Persistent p, SimpleSender s, AbstractVariable? v) => (Not ^ GetPredicate(me, p, s, v)) | (Or?.Valid(me, p, s, v) ?? false);
        protected virtual bool GetPredicate(PlayerTeam me, Persistent p, SimpleSender s, AbstractVariable? v)
        {
            return Type switch
            {
                ConditionType.SourceMe => s.TeamID == me.TeamID,
                ConditionType.SourceThis => s.TeamID == me.TeamID && s is IPeristentSupplier ips && (ips.Persistent is Character c ? c.PersistentRegion == p.PersistentRegion : ips.Persistent == p),
                ConditionType.TargetMe => v is AbstractAmountVariable aav && aav.TargetTeam == me.TeamID,
                ConditionType.TargetThis => v is AbstractAmountVariable aav && aav.TargetTeam == me.TeamID && (me.Characters.ElementAtOrDefault(p.PersistentRegion) is null ? aav.TargetIndex == me.CurrCharacter : p.PersistentRegion == aav.TargetIndex),

                ConditionType.GameStart => s is OnCharacterOnSender ocos && ocos.Start,
                ConditionType.SimpleFood => p is Character c && c.Alive && !c.Effects.Contains("minecraft:effect_full"),
                _ => throw new NotImplementedException($"Unknown Predicate In Type: {Type}")
            };
        }
    }
}
