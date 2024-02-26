namespace TCGBase
{
    public class Character : Persistent<CardCharacter>
    {
        private int _hp;
        private int _mp;
        private int _element;
        private readonly PlayerTeam _t;

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

        public bool Alive { get; private set; }
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
        internal Character(CardCharacter character, int index, PlayerTeam t) : base(character)
        {
            //多一点怎么了
            // 使用技能后对应技能的值+1，每回合行动阶段开始时清零，记录一个回合内使用技能的次数
            Data = Enumerable.Repeat(0, character.TriggerableList.Count()).ToList();

            PersistentRegion = index;
            _t = t;
            Effects = new(index, t);

            Alive = true;
            Active = true;
            HP = Card.MaxHP;
        }
        /// <summary>
        /// 被击倒角色会在异次元空间中参与结算......<br/>
        /// 此时仍然alive，但是predie
        /// </summary>
        internal void ToDieLimbo()
        {
            for (int i = 0; i < Data.Count; i++)
            {
                Data[i] = 0;
            }
            HP = 0;
            MP = 0;
            Element = 0;
        }
        internal void ToDie()
        {
            Alive = false;
        }
        internal void DieTrigger(HurtSourceSender hss, DamageVariable dv)
        {
            while (Effects.Any())
            {
                var p = Effects.First();
                if (p.CardBase.TriggerableList.TryGetValue(hss.SenderName, out var h))
                {
                    GetDelayedHandler((me, s, v) => h?.Trigger(me, this, s, v))?.Invoke(_t, hss, dv);
                }
                Effects.Destroy(0);
            }
        }
        /// <summary>
        /// not alive => null
        /// </summary>
        internal EventPersistentSetHandler? GetPersistentHandlers(AbstractSender sender)
        {
            EventPersistentSetHandler? hs = null;
            if (Alive)
            {
                if (sender is ActionUseSkillSender ss)
                {
                    if (ss.TeamID == _t.TeamIndex && PersistentRegion == ss.Character && Card.TriggerableList.TryGetValue(sender.SenderName, out var skill, ss.Skill))
                    {
                        hs += GetDelayedHandler((me, s, v) => skill.Trigger(me, this, sender, v));
                    }
                }
                else
                {
                    if (Card.TriggerableList.TryGetValue(sender.SenderName, out var h))
                    {
                        hs += GetDelayedHandler((me, s, v) => h.Trigger(me, this, sender, v));
                    }
                    hs += Effects.GetPersistentHandlers(sender);
                }
            }
            return hs;
        }
        public void AddEffect(AbstractCardBase effect) => AddEffect(new Persistent(effect));
        /// <summary>
        /// 只有活着的时候，并且添加的是普通的effect，才能添加状态<br/>
        /// 如果添加的是圣遗物或武器，还会顶掉原来的
        /// </summary>
        public void AddEffect(Persistent effect)
        {
            if (Alive)
            {
                switch (effect.CardBase.CardType)
                {
                    case CardType.Equipment:
                        if (effect.CardBase.Tags.Contains(CardTag.Artifact.ToString()))
                        {
                            Effects.DestroyFirst(p => p.CardBase.Tags.Contains(CardTag.Artifact.ToString()));
                            Effects.Add(effect);
                        }
                        else if (effect.CardBase.Tags.Contains(CardTag.Weapon.ToString()))
                        {
                            Effects.DestroyFirst(p => p.CardBase.Tags.Contains(CardTag.Weapon.ToString()));
                            Effects.Add(effect);
                        }
                        else
                        {
                            Effects.Add(effect);
                        }
                        break;
                    case CardType.Effect:
                        Effects.Add(effect);
                        break;
                }
            }
        }
    }
}
