namespace TCGBase
{
    public enum DamageSource
    {
        NoWhere,
        Character,
        Summon
    }
    public class DamageVariable : AbstractVariable
    {
        public override string VariableName => Tags.VariableTags.DAMAGE;
        public int BaseDamage { get; init; }

        /// <summary>
        /// 0-7 物理 冰水火雷岩草风<br/>
        /// -1 穿透
        /// </summary>
        public int Element { get; set; }
        /// <summary>
        /// 供effect处理增伤或者减伤<br/>
        /// 如果为穿透伤害，结算时不会加上这一项
        /// </summary>
        public int DamageModifier { get; set; }
        /// <summary>
        /// 伤害可以来源于[角色]、[召唤物]、[其他]
        /// </summary>
        public DamageSource Source { get; init; }
        /// <summary>
        /// 伤害的承受者总是[角色]
        /// </summary>
        public int TargetIndex { get; init; }
        public DamageVariable(int element, int basedamage, DamageSource source, int target)
        {
            Element = int.Clamp(element, -1, 7);
            BaseDamage = int.Max(0, basedamage);
            Source = source;
            TargetIndex = target;
            DamageModifier = 0;
        }
    }
}
