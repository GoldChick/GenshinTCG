namespace TCGBase
{
    public class DiceCostVariable : AbstractVariable
    {
        private int[] dices;
        public override string VariableName => Tags.VariableTags.DICECOST;
        public int[] Dices { get => dices; }
        public DiceCostVariable(params int[] dices)
        {
            TCGUtil.Cost.NormalizeCost(dices, out this.dices);
        }
    }
}
