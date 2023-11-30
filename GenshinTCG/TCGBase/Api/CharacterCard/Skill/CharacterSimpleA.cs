namespace TCGBase
{
    /// <summary>
    /// 没有特殊效果，只对对方出战角色造成伤害的skill
    /// </summary>
    public class CharacterSimpleA : AbstractCardSkill
    {
        private readonly int _damage;
        private readonly int _element;
        /// <param name="diceElement">默认A会消耗2杂+1有效，如果不填，则默认为element</param>
        public CharacterSimpleA(int element, int damage, int diceElement = -1)
        {
            _damage = int.Max(0, damage);
            _element = int.Clamp(element, -1, 7);
            var _costs = new int[9];
            _costs[8] = 2;
            if (diceElement > 0 && diceElement < 8)
            {
                _costs[diceElement] = 1;
            }
            else if (_element > 0)
            {
                _costs[_element] = 1;
            }
            Cost = new(_costs, _damage);
        }

        public override SkillCategory Category => SkillCategory.A;

        public override CostInit Cost { get; }

        public override void AfterUseAction(PlayerTeam me, Character c, int[] targetArgs)
        {
            me.Enemy.Hurt(new(_element, _damage, 0), this);
        }
    }
}
