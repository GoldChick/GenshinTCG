namespace TCGBase
{
    /// <summary>
    /// 没有特殊效果，只对对方出战角色造成伤害的skill
    /// </summary>
    public class CharacterSimpleSkill : AbstractCardSkill
    {
        public override CostInit Cost { get; }

        public override SkillCategory Category { get; }
        private readonly DamageVariable? _dv;
        /// <summary>
        /// skill: this<br/>
        /// playerteam: me<br/>
        /// character: skill owner<br/>
        /// int[]: possible args (nothing)
        /// </summary>
        private readonly Action<AbstractCardSkill, PlayerTeam, Character, int[]>? _skillaction;
        public CharacterSimpleSkill(SkillCategory category, CostInit cost, DamageVariable? dv = null)
        {
            Category = category;
            Cost = cost ?? new();
            _dv = dv;
        }
        public CharacterSimpleSkill(SkillCategory category, CostInit cost, Action<AbstractCardSkill, PlayerTeam, Character, int[]> skillaction, DamageVariable? dv = null) : this(category, cost, dv)
        {
            _skillaction = skillaction;
        }
        public override void AfterUseAction(PlayerTeam me, Character c, int[] targetArgs)
        {
            if (_dv != null)
            {
                me.Enemy.Hurt(_dv, this, () => _skillaction?.Invoke(this, me, c, targetArgs));
            }
            else
            {
                _skillaction?.Invoke(this, me, c, targetArgs);
            }
        }
    }
}
