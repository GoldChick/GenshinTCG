//namespace TCGBase
//{
//    public class Character
//    {
//        public AbstractCardCharacter Card { get; }
//        /// <summary>
//        /// 表示自己在team中的位置
//        /// </summary>
//        public int Index { get; init; }
//        private int _hp;
//        private int _mp;
//        private int _element;
//        private PlayerTeam _t;

//        public PersistentSet<ICardPersistent> Effects { get; init; }
//        /// <summary>
//        /// HP并不在改变时发包，而在治疗、受伤时发包
//        /// </summary>
//        public int HP
//        {
//            get { return _hp; }
//            set
//            {
//                _hp = int.Clamp(value, 0, Card.MaxHP);
//            }
//        }
//        public int MP
//        {
//            get => _mp; set
//            {
//                _mp = int.Clamp(value, 0, Card.MaxMP);
//                _t.Game.BroadCast(ClientUpdateCreate.CharacterUpdate.MPUpdate(_t.TeamIndex, Index, _mp));
//            }
//        }

//        public bool Alive;
//        /// <summary>
//        /// 濒死状态，生命值降为0，又不被“免于被击倒”治疗会使其为true<br/>
//        /// 真死了之后又为false
//        /// </summary>
//        internal bool Predie;
//        /// <summary>
//        /// 为active时可以使用技能
//        /// </summary>
//        public bool Active { get; set; }
//        public int Element
//        {
//            get => _element;
//            set
//            {
//                _element = value;
//                _t.Game.BroadCast(ClientUpdateCreate.CharacterUpdate.ElementUpdate(_t.TeamIndex, Index, _element));
//            }
//        }
//        public Character(AbstractCardCharacter character, int index, PlayerTeam t)
//        {
//            Card = character;
//            Index = index;
//            _t = t;

//            Effects = new(index, t);

//            HP = Card.MaxHP;
//            Alive = true;
//            Active = true;
//        }
//    }
//}
