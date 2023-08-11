namespace TCGBase
{
    public enum ActionType
    {
        Trival,
        ReRollDice,
        ReRollCard,
        Switch,
        UseSKill,
        UseCard,
        Blend,
        Pass,
    }
    public abstract class AbstractAction
    {
        public abstract ActionType Type { get; }
    }
}
