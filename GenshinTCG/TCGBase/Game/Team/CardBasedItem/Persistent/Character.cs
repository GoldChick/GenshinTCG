namespace TCGBase
{
    public class Character : Persistent<CardCharacter>
    {
        private int _hp;
        private int _mp;
        private int _element;
        private readonly PlayerTeam _t;

        public PersistentSet Effects { get; }
        public Dictionary<string, int> SkillCounter { get; }
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
            // 多一点怎么了
            // 使用技能后对应技能的值+1，每回合行动阶段开始时清零，记录一个回合内使用技能的次数
            SkillCounter = new();
            foreach (var triggerable in character.TriggerableList)
            {
                SkillCounter.TryAdd(triggerable.NameID, 0);
            }

            PersistentRegion = index;
            AvailableTimes = 1;

            _t = t;
            Effects = new(index, t);

            Alive = true;
            Active = true;
            HP = Card.MaxHP;
        }
        private void ResetCounter()
        {
            foreach (var triggerable in Card.TriggerableList)
            {
                SkillCounter[triggerable.NameID] = 0;
            }
        }
        /// <summary>
        /// 被击倒角色会在异次元空间中参与结算......<br/>
        /// 此时仍然alive，但是predie
        /// </summary>
        internal void ToDieLimbo()
        {
            ResetCounter();
            HP = 0;
            MP = 0;
            Element = 0;
        }
        internal void ToDie()
        {
            Alive = false;
        }
        internal void Revive()
        {
            Alive = true;
            _t.Game.EffectTrigger(new OnCharacterOnSender(_t.TeamIndex, this));
        }
        /// <summary>
        /// 死亡时只触发第一个 TODO:Check It
        /// </summary>
        internal void DieTrigger(HurtSourceSender hss, DamageVariable dv)
        {
            while (Effects.Any())
            {
                var p = Effects.First();
                if (p.CardBase.TriggerableList.TryGetValue(hss.SenderName, out var h))
                {
                    h?.Trigger(_t, this, hss, dv);
                }
                Effects.Destroy(0);
            }
        }
        /// <summary>
        /// not alive => null
        /// </summary>
        internal List<EventPersistentSetHandler> GetPersistentHandlers(AbstractSender sender)
        {
            List<EventPersistentSetHandler> hss = new();
            if (Alive)
            {
                if (sender is ITriggerableIndexSupplier indexSp)
                {
                    if (sender.TeamID == _t.TeamIndex && PersistentRegion == indexSp.SourceIndex && Card.TriggerableList.TryGetValue(sender.SenderName, out var skill, indexSp.TriggerableIndex))
                    {
                        hss.Add((s, v) => skill.Trigger(_t, this, sender, v));
                    }
                }
                else if (sender.SenderName == SenderTag.RoundStep.ToString())
                {
                    hss.Add((s, v) => ResetCounter());
                }
                else
                {
                    foreach (var it in CardBase.TriggerableList[sender.SenderName])
                    {
                        hss.Add((s, v) => it.Trigger(_t, this, sender, v));
                    }
                    hss.AddRange(Effects.GetPersistentHandlers(sender));
                }
            }
            return hss;
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
