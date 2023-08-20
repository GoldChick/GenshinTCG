namespace TCGBase
{
    public enum ActionType
    {
        Trival,
        ReRollDice,
        ReRollCard,
        ReplaceSupport,
        Switch,
        UseSKill,
        UseCard,
        Blend,
        Pass,
    }
    public class NetAction
    {
        public ActionType Type { get; }
        public int Index { get; }
        public NetAction(ActionType type, int index)
        {
            Type = type;
            Index = index;
        }
    }
}
