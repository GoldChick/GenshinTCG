namespace TCGBase
{
    public class DiceCostVariable : AbstractVariable
    {
        private int[] dices;
        public override string VariableName => Tags.VariableTags.DICECOST;
        public int[] Dices { get => dices; }
        public DiceCostVariable(params int[] dices)
        {
            if (dices.Length>=8) 
            {
                this.dices = dices[..8];
            }else
            {
                this.dices= new int[8];
                dices.CopyTo(this.dices, 0);
            }
        }
    }
}
