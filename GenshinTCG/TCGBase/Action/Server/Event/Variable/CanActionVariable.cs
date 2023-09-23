namespace TCGBase
{
    /// <summary>
    /// 通过修改这个在结算时判断是[战斗行动]还是[快速]
    /// </summary>
    public class CanActionVariable : AbstractSimpleVariable<bool>
    {
        public override string VariableName => Tags.VariableTags.CAN_ACTION;
        public CanActionVariable(bool can = true) : base(can) { }
    }
}
