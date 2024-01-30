namespace TCGBase
{
    /// <summary>
    /// 没有特殊效果，只对对方出战角色造成伤害的skill
    /// </summary>
    public class CharacterSimpleSkill : AbstractCardSkill
    {
        public override CostInit DamageCost { get; }

        public override SkillCategory DamageSkillCategory { get; }
        private readonly DamageVariable? _dv;
        /// <summary>
        /// skill: this<br/>
        /// playerteam: me<br/>
        /// character: skill owner<br/>
        /// int[]: possible args (nothing)
        /// </summary>
        private readonly Action<AbstractCardSkill, AbstractTeam, Character>? _skillaction;
        public CharacterSimpleSkill(SkillCategory category, CostInit cost, DamageVariable? dv = null)
        {
            DamageSkillCategory = category;
            DamageCost = cost ?? new();
            _dv = dv;
        }
        public CharacterSimpleSkill(SkillCategory category, CostInit cost, Action<AbstractCardSkill, AbstractTeam, Character> skillaction, DamageVariable? dv = null) : this(category, cost, dv)
        {
            _skillaction = skillaction;
        }
        public override void AfterUseAction(AbstractTeam me, Character c)
        {
            if (_dv != null)
            {
                me.DoDamage(_dv, this, () => _skillaction?.Invoke(this, me, c));
            }
            else
            {
                _skillaction?.Invoke(this, me, c);
            }
        }
    }
}
