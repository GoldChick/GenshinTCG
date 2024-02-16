namespace TCGBase
{
    public class HealVariable : AbstractTargetVariable
    {
        public int Amount { get => _value; set => _value = int.Max(0, value); }
        /// <summary>
        /// 通过public方法创建的heal的targetindex为相对坐标<br/>
        /// 在内部转换为绝对坐标运算
        /// </summary>
        public HealVariable(int baseamount, int relativeTarget = 0, TargetArea targetArea = TargetArea.TargetOnly) : base(DamageSource.Direct, TargetTeam.Me, relativeTarget, true, targetArea)
        {
            Amount = baseamount;
        }
        internal HealVariable(DamageSource source, int baseamount, int absoluteTarget = 0, TargetArea targetArea = TargetArea.TargetOnly) : base(source, TargetTeam.Me, absoluteTarget, false, targetArea)
        {
            Amount = baseamount;
        }
        //TODO: heal team? heal还需要很多现代化改造
    }
}
