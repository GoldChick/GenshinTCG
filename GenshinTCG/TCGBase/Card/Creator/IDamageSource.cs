namespace TCGBase
{
    /// <summary>
    /// 表示可作为伤害的来源的东西:
    /// </summary>
    public interface IDamageSource
    {
        public CostInit Cost { get; }
    }
}
