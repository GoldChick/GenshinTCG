namespace TCGBase
{
    public class Character : Persistent<AbstractCardCharacter>
    {
        public override ICardPersistent CardBase => Card;
        private int _hp;
        private int _mp;
        private int _element;
        private readonly PlayerTeam _t;

        public PersistentSet<ICardPersistent> Effects { get; init; }
        /// <summary>
        /// HP并不在改变时发包，而在治疗、受伤时发包
        /// </summary>
        public int HP
        {
            get { return _hp; }
            set
            {
                _hp = int.Clamp(value, 0, Card.MaxHP);
            }
        }
        public int MP
        {
            get => _mp; set
            {
                _mp = int.Clamp(value, 0, Card.MaxMP);
                _t.Game.BroadCast(ClientUpdateCreate.CharacterUpdate.MPUpdate(_t.TeamIndex, PersistentRegion, _mp));
            }
        }

        public bool Alive;
        /// <summary>
        /// 濒死状态，生命值降为0，又不被“免于被击倒”治疗会使其为true<br/>
        /// 真死了之后又为false
        /// </summary>
        internal bool Predie;
        public int Element
        {
            get => _element;
            set
            {
                _element = value;
                _t.Game.BroadCast(ClientUpdateCreate.CharacterUpdate.ElementUpdate(_t.TeamIndex, PersistentRegion, _element));
            }
        }
        /// <summary>
        /// 使用技能后对应技能的值+1，每回合行动阶段开始时清零，记录一个回合内使用技能的次数
        /// </summary>
        public List<int> SkillCounter { get; }

        public Character(AbstractCardCharacter character, int index, PlayerTeam t) : base(character)
        {
            Card = character;
            SkillCounter = Enumerable.Repeat(0, character.Skills.Length).ToList();
            Data = SkillCounter;

            PersistentRegion = index;
            _t = t;
            Effects = new(index, t);

            HP = Card.MaxHP;
            Alive = true;
            Active = true;
        }
    }
}
