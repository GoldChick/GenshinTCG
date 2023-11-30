namespace TCGBase
{
    /// <summary>
    /// 没有特殊效果，只对对方出战角色造成伤害的skill
    /// </summary>
    public class CharacterSimpleSkill : AbstractCardSkill
    {
        public override CostInit Cost { get; }

        public override SkillCategory Category { get; }
        private readonly DamageVariable[] _dvs;
        /// <summary>
        /// skill: this<br/>
        /// playerteam: me<br/>
        /// character: skill owner<br/>
        /// int[]: possible args (nothing)
        /// </summary>
        private readonly Action<AbstractCardSkill, PlayerTeam, Character, int[]>? _skillaction;
        public CharacterSimpleSkill(SkillCategory category, CostInit cost, params DamageVariable[] dvs)
        {
            Category = category;
            Cost = cost ?? new();
            _dvs = dvs ?? Array.Empty<DamageVariable>();
        }
        public CharacterSimpleSkill(SkillCategory category, CostInit cost, Action<AbstractCardSkill, PlayerTeam, Character, int[]> skillaction, params DamageVariable[] dvs) : this(category, cost, dvs)
        {
            _skillaction = skillaction;
        }
        public override void AfterUseAction(PlayerTeam me, Character c, int[] targetArgs)
        {
            if (_dvs.Any())
            {
                me.MultiHurt(_dvs, this, () => _skillaction?.Invoke(this, me, c, targetArgs));
            }
            else
            {
                _skillaction?.Invoke(this, me, c, targetArgs);
            }
        }
    }
}
