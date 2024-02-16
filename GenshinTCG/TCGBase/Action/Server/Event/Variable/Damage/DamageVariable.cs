namespace TCGBase
{
    public enum DamageElement
    {
        Trival = 0,
        Cryo = 1,
        Hydro = 2,
        Pyro = 3,
        Electro = 4,
        Geo = 5,
        Dendro = 6,
        Anemo = 7,
        Pierce = 8,
    }
    /// <summary>
    /// 用于表明是直接伤害，还是间接伤害(扩散、超导等引发的额外伤害)
    /// </summary>
    public enum DamageSource
    {
        Direct,
        Indirect
    }
    public enum TargetTeam
    {
        Enemy,
        Me
    }
    public enum TargetArea
    {
        TargetOnly,
        TargetExcept
    }
    public class DamageVariable : AbstractTargetVariable
    {
        private DamageElement _element;
        /// <summary>
        /// [物理伤害]和[元素伤害]能够增加
        /// </summary>
        public int Damage { get => _value; set => _value = int.Max(0, value); }
        /// <summary>
        /// 物理 冰水火雷岩草风 穿透<br/>
        /// 只有[物理伤害]能被附魔
        /// </summary>
        public DamageElement Element
        {
            get => _element; set
            {
                if (_element == DamageElement.Trival)
                {
                    _element = value;
                }
            }
        }
        /// <summary>
        /// 伤害触发的反应类型，仅在伤害结算时获得，只读
        /// </summary>
        public ReactionTags Reaction { get; internal set; }
        /// <summary>
        /// 伤害结算过程中可能创建的子伤害，当然创建之初为相对坐标（<br/>
        /// 伤害将以宽度优先的方式，从主伤害开始结算，
        /// </summary>
        internal DamageVariable? SubDamage { get; set; }
        protected override AbstractTargetVariable? Sub => SubDamage;
        /// <summary>
        /// 通过public方法创建的dmg的targetindex为相对坐标(相对出战角色)
        /// </summary>
        public DamageVariable(DamageElement element, int basedamage, TargetTeam targetTeam) : this(element, basedamage, 0, TargetArea.TargetOnly, null, targetTeam)
        {
        }
        /// <summary>
        /// 通过public方法创建的dmg的targetindex为相对坐标(相对出战角色)
        /// </summary>
        public DamageVariable(DamageElement element, int basedamage, int relativeTarget = 0, TargetArea targetArea = TargetArea.TargetOnly, DamageVariable? subdamage = null, TargetTeam targetTeam = TargetTeam.Enemy) : base(DamageSource.Direct, targetTeam, relativeTarget, true, targetArea)
        {
            Element = element;
            Damage = int.Max(0, basedamage);
            SubDamage = subdamage;
        }
        /// <summary>
        /// 通过record创建dmg,targetindex为相对坐标
        /// </summary>
        internal DamageVariable(DamageRecord record) : this(record.Element, record.Amount, record.TargetIndexOffset, record.TargetArea, null, record.Team)
        {
        }
        /// <summary>
        /// 通过internal方法创建的dmg的targetindex为绝对坐标，并且没有子伤害
        /// </summary>
        internal DamageVariable(DamageSource source, DamageElement element, int basedamage, int absoluteTarget, TargetArea targetArea, TargetTeam targetTeam) : base(source, targetTeam, absoluteTarget, false, targetArea)
        {
            Element = element;
            Damage = int.Max(0, basedamage);
            SubDamage = null;
        }
    }
}
