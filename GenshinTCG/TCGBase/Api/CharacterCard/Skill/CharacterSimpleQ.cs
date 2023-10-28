namespace TCGBase
{
    /// <summary>
    /// 没有特殊效果，只对对方出战角色造成伤害的skill
    /// </summary>
    public class CharacterSimpleQ : AbstractCardSkill
    {
        private readonly string _name;
        private readonly int[] _costs;
        private readonly int _damage;
        private readonly int _element;
        /// <param name="diceElement">如果不填，则默认消耗element；如果element再不为某种元素，则为同</param>
        /// <param name="diceElement">至少消耗3有效</param>
        public CharacterSimpleQ(string nameID, int element, int damage, int diceElement = -1, int diceNum = 3)
        {
            _name = nameID;
            _damage = int.Max(0, damage);
            _element = int.Clamp(element, -1, 7);
            _costs = new int[8];
            diceNum = int.Max(3, diceNum);
            if (diceElement > 0 && diceElement < 8)
            {
                _costs[diceElement] = diceNum;
            }
            else if (_element > 0)
            {
                _costs[_element] = diceNum;
            }
            else
            {
                _costs[0] = diceNum;
            }
        }
        public override int[] Costs => _costs;

        public override bool CostSame => true;

        public override string NameID => _name;

        public override SkillCategory Category => SkillCategory.Q;

        public override void AfterUseAction(PlayerTeam me, Character c, int[]? targetArgs = null)
        {
            me.Enemy.Hurt(new(_element, _damage, 0), this);
        }
    }
}
