
namespace TCGBase
{
    public class Character : Persistent<AbstractCardCharacter>
    {
        public override AbstractCardBase CardBase => Card;
        private int _hp;
        private int _mp;
        private int _element;
        private readonly AbstractTeam _t;

        public PersistentSet<AbstractCardBase> Effects { get; }
        /// <summary>
        /// HP并不在改变时发包，而在治疗、受伤时发包
        /// </summary>
        public int HP
        {
            get { return _hp; }
            set
            {
                if (Alive)
                {
                    _hp = int.Clamp(value, 0, Card.MaxHP);
                }
            }
        }
        public int MP
        {
            get => _mp; set
            {
                if (Alive)
                {
                    _mp = int.Clamp(value, 0, Card.MaxMP);
                    _t.Game.BroadCast(ClientUpdateCreate.CharacterUpdate.MPUpdate(_t.TeamIndex, PersistentRegion, _mp));
                }
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
                if (Alive)
                {
                    _element = value;
                    _t.Game.BroadCast(ClientUpdateCreate.CharacterUpdate.ElementUpdate(_t.TeamIndex, PersistentRegion, _element));
                }
            }
        }
        /// <summary>
        /// 使用技能后对应技能的值+1，每回合行动阶段开始时清零，记录一个回合内使用技能的次数
        /// </summary>
        public List<int> SkillCounter { get; }

        internal Character(AbstractCardCharacter character, int index, PlayerTeam t) : base(character)
        {
            Card = character;
            //多一点怎么了
            SkillCounter = Enumerable.Repeat(0, character.TriggerableList.Count()).ToList();
            Data = SkillCounter;

            PersistentRegion = index;
            _t = t;
            Effects = new(index, t);

            HP = Card.MaxHP;
            Alive = true;
            Active = true;
        }
        /// <summary>
        /// 被击倒角色会在异次元空间中参与结算......<br/>
        /// 此时仍然alive，但是predie
        /// </summary>
        internal Character ToDieLimbo(EmptyTeam emptyTeam)
        {
            Character limbo_c = new(this, emptyTeam);
            for (int i = 0; i < SkillCounter.Count; i++)
            {
                SkillCounter[i] = 0;
            }
            HP = 0;
            MP = 0;
            Element = 0;
            Predie = true;
            return limbo_c;
        }
        /// <summary>
        /// for copy
        /// </summary>
        internal Character(Character die_character, EmptyTeam emptyTeam) : base(die_character.Card)
        {
            Card = die_character.Card;
            SkillCounter = die_character.SkillCounter.ToList();
            Data = SkillCounter;
            PersistentRegion = die_character.PersistentRegion;
            _t = emptyTeam;
            Effects = die_character.Effects;
            Alive = false;
            Active = false;
            //TODO: check it
        }
        internal EventPersistentSetHandler? GetPersistentHandlers(AbstractSender sender)
        {
            EventPersistentSetHandler? hs = null;
            //TODO: check it 如何触发自己？
            //if (Card.TriggerList.TryGetValue(sender.SenderName, out var h))
            //{
            //    hs += (me, s, v) => h.Trigger(me, this, sender, v);
            //}
            hs += Effects.GetPersistentHandlers(sender);
            return hs;
        }
        /// <summary>
        /// 只有活着的时候，并且添加的是普通的effect，才能添加状态<br/>
        /// 如果添加的是圣遗物或武器，还会顶掉原来的
        /// </summary>
        public void AddEffect(Persistent<AbstractCardBase> effect)
        {
            if (Alive && !Predie)
            {
                //TODO:弃置
                if (effect.CardBase.Tags.Contains(CardTag.Artifact.ToString()))
                {
                    Effects.TryRemove(-2);
                    Effects.Add(effect);
                }
                else if (effect.CardBase.Tags.Contains(CardTag.Weapon.ToString()))
                {
                    Effects.TryRemove(-1);
                    Effects.Add(effect);
                }
                else if (effect.CardBase.CardType == CardType.Effect)
                {
                    Effects.Add(effect);
                }
            }
        }
    }
}
