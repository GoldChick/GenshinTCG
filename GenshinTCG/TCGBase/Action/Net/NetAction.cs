namespace TCGBase
{
    public enum ActionType
    {
        Trival,

        ReRollDice,

        ReRollCard,

        ReplaceSupport,
        /// <summary>
        /// Index:Target Character Index
        /// </summary>
        Switch,
        /// <summary>
        /// Index:Target Character Index <br/>
        /// 出战、死亡导致的免费、快速切人
        /// </summary>
        SwitchForced,
        /// <summary>
        /// Index:Skill Index 
        /// </summary>
        UseSKill,
        /// <summary>
        /// Index:Card Index
        /// </summary>
        UseCard,
        //Target:Card
        Blend,
        Pass,
    }
    public class NetAction
    {
        public ActionType Type { get; }
        /// <summary>
        /// 某Action对应的Index含义见其注释(没有就是没有)
        /// </summary>
        public int Index { get; }

        public NetAction(ActionType type, int index = 0)
        {
            Type = type;
            Index = index;
        }
    }
}
