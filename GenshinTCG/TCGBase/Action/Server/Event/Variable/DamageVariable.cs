namespace TCGBase
{
    public enum DamageSource
    {
        /// <summary>
        /// 来源不明的伤害，如[轰轰火花]\[愚人众伏兵]
        /// </summary>
        NoWhere,
        /// <summary>
        /// 直接由[对方]的[角色技能]造成的伤害
        /// </summary>
        Character,
        /// <summary>
        /// 直接由[对方]的[召唤物]造成的伤害
        /// </summary>
        Summon,
        /// <summary>
        /// [对方]造成的，但不属于以上两种来源的伤害，如[扩散]\[旋火轮]
        /// </summary>
        Addition
    }
    public class DamageVariable : AbstractVariable
    {
        private int _damage;
        private int _element;
        public int Damage { get => _damage; set => _damage = int.Max(0, value); }

        /// <summary>
        /// 0-7 物理 冰水火雷岩草风<br/>
        /// -1 穿透<br/>
        /// 只有物理伤害能被附魔
        /// </summary>
        public int Element
        {
            get => _element; set
            {
                if (_element == 0)
                {
                    _element = int.Clamp(value, -1, 7);
                }
            }
        }
        /// <summary>
        /// 此处为伤害的<b>直接、额外来源</b>，比如扩散伤害在此处视为[Addition]，但是根本来源不一定，其在一开始就被创建<br/>
        /// 根本来源<see cref="IDamageSource"/>
        /// </summary>
        public DamageSource DirectSource { get; internal set; }
        /// <summary>
        /// 伤害的承受者总是[角色]
        /// </summary>
        public int TargetIndex { get; set; }
        /// <summary>
        /// 为true时代表是相对坐标<br/>
        /// 在各种TCGMod中创建的伤害都应该为相对坐标<br/>
        /// 结算伤害时，会转化为绝对坐标
        /// </summary>
        public bool TargetRelative { get; init; }
        /// <summary>
        /// 为true时，改为对target以外的所有角色造成伤害<br/>
        /// </summary>
        public bool TargetExcept { get; init; }
        /// <summary>
        /// 伤害触发的反应类型，仅在伤害结算时获得，只读
        /// </summary>
        public ReactionTags Reaction { get; internal set; }
        /// <summary>
        /// 通过public方法创建的dmg的targetindex为相对坐标
        /// </summary>
        public DamageVariable(int element, int basedamage, int relativeTarget=0, bool targetExcept = false)
        {
            Element = int.Clamp(element, -1, 7);
            Damage = int.Max(0, basedamage);
            DirectSource = DamageSource.NoWhere;
            TargetIndex = relativeTarget;
            TargetRelative = true;
            TargetExcept = targetExcept;
        }
        /// <summary>
        /// 通过internal方法创建的dmg的targetindex为绝对坐标
        /// </summary>
        internal DamageVariable(DamageSource source, int element, int basedamage, int absoluteTarget, bool targetExcept = false)
        {
            Element = int.Clamp(element, -1, 7);
            Damage = int.Max(0, basedamage);
            DirectSource = source;
            TargetIndex = absoluteTarget;
            TargetRelative = false;
            TargetExcept = targetExcept;
        }
    }
}
