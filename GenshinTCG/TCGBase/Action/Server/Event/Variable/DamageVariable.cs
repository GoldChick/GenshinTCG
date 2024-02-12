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
    public enum DamageTargetTeam
    {
        Enemy,
        Me
    }
    public enum DamageTargetArea
    {
        TargetOnly,
        TargetExcept
    }
    public class DamageVariable : AbstractVariable
    {
        private int _damage;
        private DamageElement _element;
        /// <summary>
        /// [物理伤害]和[元素伤害]能够增加
        /// </summary>
        public int Damage
        {
            get => _damage; set
            {
                if (_element >= 0)
                {
                    _damage = int.Max(0, value);
                }
            }
        }
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
        public DamageSource Direct { get; private set; }
        public DamageTargetTeam DamageTargetTeam { get; private set; }
        /// <summary>
        /// 目标角色的index，绝对坐标还是相对坐标参见<see cref="TargetRelative"/>
        /// </summary>
        public int TargetIndex { get; set; }
        /// <summary>
        /// 为true时代表是相对坐标<br/>
        /// 在各种TCGMod中创建的伤害都应该为相对坐标<br/>
        /// 结算伤害时，会转化为绝对坐标
        /// </summary>
        internal bool TargetRelative { get; private set; }
        /// <summary>
        /// 为TargetExcept时，改为对target以外的所有角色造成伤害<br/>
        /// </summary>
        public DamageTargetArea TargetArea { get; init; }
        /// <summary>
        /// 伤害触发的反应类型，仅在伤害结算时获得，只读
        /// </summary>
        public ReactionTags Reaction { get; internal set; }
        /// <summary>
        /// 伤害结算过程中可能创建的子伤害，当然创建之初为相对坐标（<br/>
        /// 伤害将以宽度优先的方式，从主伤害开始结算，
        /// </summary>
        internal DamageVariable? SubDamage { get; set; }
        /// <summary>
        /// 通过public方法创建的dmg的targetindex为相对坐标(相对出战角色)
        /// </summary>
        public DamageVariable(DamageElement element, int basedamage, DamageTargetTeam damageTargetCategory) : this(element, basedamage, 0, DamageTargetArea.TargetOnly, null, damageTargetCategory)
        {
        }
        /// <summary>
        /// 通过public方法创建的dmg的targetindex为相对坐标(相对出战角色)
        /// </summary>
        public DamageVariable(DamageElement element, int basedamage, int relativeTarget = 0, DamageTargetArea targetArea = DamageTargetArea.TargetOnly, DamageVariable? subdamage = null, DamageTargetTeam damageTargetCategory = DamageTargetTeam.Enemy)
        {
            Element = element;
            Damage = int.Max(0, basedamage);
            Direct = DamageSource.Direct;
            DamageTargetTeam = damageTargetCategory;
            TargetIndex = relativeTarget;
            TargetRelative = true;
            TargetArea = targetArea;
            SubDamage = subdamage;
        }
        /// <summary>
        /// 通过record创建dmg,targetindex为相对坐标
        /// </summary>
        internal DamageVariable(DamageRecord record) : this(record.Element, record.Amount, record.TargetIndexOffset, record.TargetArea, null, record.TargetTeam)
        {
        }
        /// <summary>
        /// 通过internal方法创建的dmg的targetindex为绝对坐标，并且没有子伤害
        /// </summary>
        internal DamageVariable(DamageSource source, DamageElement element, int basedamage, int absoluteTarget, DamageTargetArea targetArea, DamageTargetTeam damageTargetCategory)
        {
            Element = element;
            Damage = int.Max(0, basedamage);
            Direct = source;
            DamageTargetTeam = damageTargetCategory;
            TargetIndex = absoluteTarget;
            TargetRelative = false;
            TargetArea = targetArea;
            SubDamage = null;
        }
        /// <summary>
        /// 如果是相对坐标，就改成绝对坐标<br/>
        /// me为产生这个伤害的队伍
        /// </summary>
        internal void ToAbsoluteIndex(PlayerTeam me)
        {
            if (TargetRelative)
            {
                var team = DamageTargetTeam == DamageTargetTeam.Enemy ? me.Enemy : me;
                if (TargetIndex % team.Characters.Where(c => c.Alive).Count() != 0)
                {
                    do
                    {
                        TargetIndex = (TargetIndex + team.CurrCharacter) % team.Characters.Length;
                    }
                    while (!team.Characters[TargetIndex].Alive);
                }
                TargetRelative = false;
                SubDamage?.ToAbsoluteIndex(team);
            }
        }
    }
}
