namespace TCGBase
{
    public class HealVariable : AbstractVariable
    {
        private int heal;
        public int Amount { get => heal; set => heal = int.Max(0, value); }
        /// <summary>
        /// 此处为治疗的<b>直接、额外来源</b>
        /// </summary>
        public DamageSource DirectSource { get; internal set; }
        /// <summary>
        /// 治疗的承受者总是[角色]
        /// </summary>
        public int TargetIndex { get; set; }
        /// <summary>
        /// 为true时，改为对target以外的所有角色造成治疗<br/>
        /// </summary>
        public bool TargetExcept { get; init; }
        /// <summary>
        /// 通过public方法创建的heal的targetindex为相对坐标<br/>
        /// 在内部转换为绝对坐标运算
        /// </summary>
        public HealVariable(int baseamount, int relativeTarget = 0, bool targetArea = false)
        {
            Amount = int.Max(0, baseamount);
            DirectSource = DamageSource.Direct;
            TargetIndex = relativeTarget;
            TargetExcept = targetArea;
        }

        internal HealVariable(DamageSource source, int baseamount, int relativeTarget = 0, bool targetArea = false)
        {
            Amount = int.Max(0, baseamount);
            DirectSource = source;
            TargetIndex = relativeTarget;
            TargetExcept = targetArea;
        }
    }
}
