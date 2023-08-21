namespace TCGBase
{
    public enum ActionType
    {
        Trival,

        ReRollDice,

        ReRollCard,

        ReplaceSupport,
        //Target:Character_Me
        Switch,
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
        public int Index { get; }
        public NetAction(ActionType type, int index = 0)
        {
            Type = type;
            Index = index;
        }
    }
}
