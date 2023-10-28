namespace TCGBase
{
    /// <summary>
    /// 额外召唤一个召唤物，只对对方出战角色造成伤害的skill
    /// </summary>
    public class CharacterEffectE : AbstractCardSkill
    {
        private readonly bool _doDamage;
        private readonly bool _oncharacter;
        private readonly AbstractCardPersistentEffect _effect;
        private readonly string _name;
        private readonly int[] _costs;
        private readonly int _damage;
        private readonly int _element;
        /// <param name="diceElement">默认E会消耗3有效；如果不填，则默认为element；如果element不为某种元素，则为3白</param>
        public CharacterEffectE(string nameID, AbstractCardPersistentEffect effect, bool onCharacter = true, int diceElement = -1)
        {
            _name = nameID;
            _effect = effect;
            _doDamage = false;
            _oncharacter = onCharacter;
            _costs = new int[8];
            if (diceElement > 0 && diceElement < 8)
            {
                _costs[diceElement] = 3;
            }
            else
            {
                _costs[0] = 3;
            }
        }
        /// <param name="diceElement">默认E会消耗3有效；如果不填，则默认为element；如果element不为某种元素，则为3白</param>
        public CharacterEffectE(string nameID, int element, int damage, AbstractCardPersistentEffect effect, bool onCharacter = true, int diceElement = -1)
        {
            _name = nameID;
            _effect = effect;
            _doDamage = true;
            _oncharacter = onCharacter;
            _damage = int.Max(0, damage);
            _element = int.Clamp(element, -1, 7);
            _costs = new int[8];
            if (diceElement > 0 && diceElement < 8)
            {
                _costs[diceElement] = 3;
            }
            else if (_element > 0)
            {
                _costs[_element] = 3;
            }
            else
            {
                _costs[0] = 3;
            }
        }
        public override int[] Costs => _costs;

        public override bool CostSame => true;

        public override string NameID => _name;

        public override SkillCategory Category => SkillCategory.E;

        public override void AfterUseAction(PlayerTeam me, Character c, int[]? targetArgs = null)
        {
            if (_doDamage)
            {
                me.Enemy.Hurt(new(_element, _damage, 0), this);
            }
            me.AddPersistent(_effect, _oncharacter ? c.Index : -1);
        }
    }
}
