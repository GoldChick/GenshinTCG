namespace TCGBase
{
    /// <summary>
    /// 通过修改这个在结算时判断是[战斗行动]还是[快速]
    /// </summary>
    public class FastActionVariable : AbstractVariable
    {
        public override string VariableName => Tags.VariableTags.FAST_ACTION;
        public bool Fast { get; set; }
        public FastActionVariable(bool fast = false) 
        {
            Fast = fast;
        }
    }
}
