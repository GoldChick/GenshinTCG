namespace TCGBase
{
    public class DiceCostVariable : AbstractVariable
    {
        private int[] dices;

        public override string VariableName => Tags.VariableTags.DICE_COST;
        public int[] Dices { get => dices; }
        public DiceCostVariable(params int[] dices)
        {
            TCGUtil.Normalize.CostNormalize(dices, out this.dices);
        }
    }
}
