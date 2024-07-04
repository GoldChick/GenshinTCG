using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum OperationType
    {
        /// <summary>
        /// 无意义...
        /// </summary>
        Trivial,
        /// <summary>
        /// Index:0表示不重投，1表示重投<br/>
        /// 选择任意骰子重投
        /// </summary>
        ReRollDice,
        /// <summary>
        /// Index:0表示不重投，1表示重投<br/>
        /// 选择任意卡牌重投
        /// </summary>
        ReRollCard,

        /// <summary>
        /// Index:Target Character Index
        /// </summary>
        Switch,
        /// <summary>
        /// Index:Skill Index 
        /// </summary>
        UseSKill,
        /// <summary>
        /// Index:Card Index
        /// </summary>
        UseCard,
        /// <summary>
        /// Index:Card Index<br/>
        /// CostArg in NetEvent:Dice Cost(only one)
        /// </summary>
        Blend,
        /// <summary>
        /// 切换到对面行动，但是不结束
        /// </summary>
        Break,
        /// <summary>
        /// 结束回合
        /// </summary>
        Pass,
    }
    public class NetOperation
    {
        public OperationType Type { get; }
        /// <summary>
        /// 某Action对应的Index含义见其注释(没有就是没有)
        /// </summary>
        public int Index { get; }
        [JsonConstructor]
        public NetOperation(OperationType type, int index = 0)
        {
            Type = type;
            Index = index;
        }
    }
}
