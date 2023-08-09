using TCGBase;

namespace TCGAI
{
    public enum AIEventType
    {
        Trival,
        ReRollDice,
        ReRollCard,
        ReplaceAssist,
        ReplaceSummon,
        Switch,
        UseSKill,
        UseCard,
        Blend,
        Pass,
    }
    /// <summary>
    /// 供编写AI使用<br/>
    /// 为防止作弊，AI编写时不能够直接创造Event
    /// </summary>
    public class AIEvent : GlobalEventBase
    {
        public AIEventType AIEventType { get; init; }
        public int MainArg { get; init; }
        public int TargetArg { get; init; }
        public int[] DiceArgs { get; init; }
        public AIEvent(AIEventType aIEventType, int mainArg, params int[] subArgs)
        {
            AIEventType = aIEventType;
            MainArg = mainArg;
            TargetArg = -1;
            DiceArgs = subArgs;
        }
        public AIEvent(int targetARg, AIEventType aIEventType, int mainArg, params int[] subArgs)
        {
            AIEventType = aIEventType;
            MainArg = mainArg;
            TargetArg = targetARg;
            DiceArgs = subArgs;
        }
    }
    /// <summary>
    /// 供AI调用的产生事件<br/>
    /// 只负责产生，校验由服务端计算
    /// </summary>
    public static class CreateEvent
    {
        /// <summary>
        /// index为绝对位置<br/>
        /// dices为选中的骰子
        /// </summary>
        public static AIEvent Switch(int index, params int[] dices) => new(AIEventType.Switch, index, dices);
        /// <summary>
        /// index为当前角色技能的索引<br/>
        /// dices为选中的骰子<br/>
        /// 默认技能作用的主体在对方的出战角色
        /// </summary>
        public static AIEvent UseSKill(int index, params int[] dices) => new(AIEventType.UseSKill, index, dices);
        public static AIEvent Pass() => new(AIEventType.Pass, 0);
    }

}
