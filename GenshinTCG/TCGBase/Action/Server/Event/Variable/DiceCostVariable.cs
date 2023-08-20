﻿namespace TCGBase
{
    public class DiceCostVariable : AbstractVariable
    {
        public override string VariableName => Tags.VariableTags.DICE_COST;
        public Cost Cost { get; set; }

        public DiceCostVariable(Cost? cost)
        {
            Cost = cost ?? new(false, 0);
        }
    }
}
