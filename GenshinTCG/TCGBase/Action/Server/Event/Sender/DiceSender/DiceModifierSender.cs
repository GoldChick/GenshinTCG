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
        public Character? Character { get; }
        public ICostable Source { get; }
        internal DiceModifierSender(int teamID, AbstractCardAction cardAction) : base(teamID)
        {
            Character = null;
            Source = cardAction;
        }
        internal DiceModifierSender(int teamID, Character character, ICostable skill) : base(teamID)
        {
            Character = character;
            Source = skill;
        }
        internal DiceModifierSender(int teamID) : base(teamID)
        {
            Character = null;
            Source = new SwitchCost();
        }
    }
    public class SwitchCost : ICostable
    {
        public CostInit Cost => new CostCreate().Add(ElementCategory.Void, 1).ToCostInit();
    }
}
