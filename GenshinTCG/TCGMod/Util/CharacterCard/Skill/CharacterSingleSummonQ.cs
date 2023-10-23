using TCGCard;
using TCGGame;
namespace TCGMod
{
    /// <summary>
    /// 额外召唤一个召唤物，只对对方出战角色造成伤害的skill
    /// </summary>
    public class CharacterSingleSummonQ : AbstractCardSkill, ISinglePersistentProvider<AbstractCardPersistentSummon>
    {
        private readonly bool _doDamage;
        private readonly AbstractCardPersistentSummon _summon;
        private readonly string _name;
        private readonly int[] _costs;
        private readonly int _damage;
        private readonly int _element;
        /// <param name="diceElement">默认E会消耗3有效；如果不填，则默认为element；如果element不为某种元素，则为3白</param>
        public CharacterSingleSummonQ(string nameID, AbstractCardPersistentSummon summon, int diceElement = -1, int diceNum = 3)
        {
            _name = nameID;
            _summon = summon;
            _doDamage = false;
            _costs = new int[8];
            diceNum = int.Max(diceNum, 3);
            if (diceElement > 0 && diceElement < 8)
            {
                //TODO:应该是3，但是for test
                _costs[diceElement] = 0;
            }
            else
            {
                _costs[0] = diceNum;
            }
        }
        /// <param name="diceElement">默认E会消耗3有效；如果不填，则默认为element；如果element不为某种元素，则为3白</param>
        public CharacterSingleSummonQ(string nameID, int element, int damage, AbstractCardPersistentSummon summon, int diceElement = -1, int diceNum = 3)
        {
            _name = nameID;
            _summon = summon;
            _doDamage = true;
            _damage = int.Max(0, damage);
            _element = int.Clamp(element, -1, 7);
            _costs = new int[8];
            diceNum = int.Max(diceNum, 3);
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

        public AbstractCardPersistentSummon PersistentPool => _summon;

        public override SkillCategory Category => SkillCategory.Q;

        public override void AfterUseAction(PlayerTeam me, Character c, int[]? targetArgs = null)
        {
            if (_doDamage)
            {
                me.Enemy.Hurt(new(_element, _damage, 0), this);
            }
        }

    }
}
