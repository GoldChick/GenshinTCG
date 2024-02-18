namespace TCGBase
{
    internal enum DiceModifierType
    {
        Void,
        Color,
        Any,
        If
    }
    public class DiceModifierSender : AbstractSender
    {
        public override string SenderName => DiceModType.ToString();
        internal DiceModifierType DiceModType { get; set; }
        public bool RealAction { get; }
        public Character? Character { get; }
        public ICostable Source { get; }
        internal DiceModifierSender(int teamID, AbstractCardAction cardAction, bool realAction) : base(teamID)
        {
            Character = null;
            Source = cardAction;
            RealAction = realAction;
        }
        internal DiceModifierSender(int teamID, Character character, ICostable skill, bool realAction) : base(teamID)
        {
            Character = character;
            Source = skill;
            RealAction = realAction;
        }
        internal DiceModifierSender(int teamID, bool realAction) : base(teamID)
        {
            Character = null;
            Source = new SwitchCost();
            RealAction = realAction;
        }
    }
    public class SwitchCost : ICostable
    {
        public CostInit Cost => new CostCreate().Add(ElementCategory.Void, 1).ToCostInit();
    }
}
