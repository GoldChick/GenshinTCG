namespace TCGBase
{
    public class DiceCostVariable : AbstractVariable
    {
        public override string VariableName => VariableTags.DICE_COST;
        public DiceCost Cost { get; set; }

        public DiceCostVariable(DiceCost? cost)
        {
            Cost = cost ?? new(false, 0);
        }
    }
}
