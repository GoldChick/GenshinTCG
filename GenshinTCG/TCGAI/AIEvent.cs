using TCGBase;

namespace TCGAI
{
    public enum AIEventType
    {
        Trival,
        ReRollDice,
        ReRollCard,
        ReplaceAssist,
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
}
