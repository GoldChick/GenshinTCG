namespace TCGBase
{
    /// <summary>
    /// 没有特殊效果，只对对方出战角色造成伤害的skill
    /// </summary>
    public class CharacterSimpleA : AbstractCardSkill
    {
        private readonly string _name;
        private readonly int[] _costs;
        private readonly int _damage;
        private readonly int _element;
        /// <param name="diceElement">默认A会消耗2杂+1有效，如果不填，则默认为element</param>
        public CharacterSimpleA(string nameID, int element, int damage, int diceElement = -1)
        {
            _name = nameID;
            _damage = int.Max(0, damage);
            _element = int.Clamp(element, -1, 7);
            _costs = new int[8];
            _costs[0] = 2;
            if (diceElement > 0 && diceElement < 8)
            {
                _costs[diceElement] = 1;
            }
            else if (_element > 0)
            {
                _costs[_element] = 1;
            }
        }
        public override int[] Costs => _costs;

        public override bool CostSame => false;

        public override string NameID => _name;

        public override SkillCategory Category => SkillCategory.A;

        public override void AfterUseAction(PlayerTeam me, Character c, int[]? targetArgs = null)
        {
            me.Enemy.Hurt(new(_element, _damage, 0), this);
        }
    }
}
