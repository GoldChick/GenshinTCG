namespace TCGBase
{
    public partial class PlayerTeam
    {
        internal void AddEquipment<T>(T equip, int target) where T : ICardPersistent
        {
            var es = Characters[int.Clamp(target, 0, Characters.Length - 1)].Effects;
            es.TryRemove(typeof(T));
            es.Add(new(equip));
        }
        /// <summary>
        /// 根据对于[出战角色]的[相对坐标]来附着[角色状态]
        /// </summary>
        public void AddPersonalEffect(ICardPersistent per, int relativeIndex = 0, AbstractPersistent? bind = null)
        {
            Character cha = Characters[(relativeIndex + CurrCharacter) % Characters.Length];
            if (cha.Alive)
            {
                cha.Effects.Add(new(per, bind));
            }
        }
        /// <summary>
        /// 根据对于[出战角色]的[相对坐标]来附着[已经存在]的[角色状态]<br/>
        /// <b>如果没有特殊需要，请使用上面的方法创建新的persistent</b>
        /// </summary>
        public void AddPersonalEffect(Persistent<ICardPersistent> per, int relativeIndex = 0)
        {
            Character cha = Characters[(relativeIndex + CurrCharacter) % Characters.Length];
            if (cha.Alive)
            {
                cha.Effects.Add(per);
            }
        }
        public void AddTeamEffect(ICardPersistent per, AbstractPersistent? bind = null) => Effects.Add(new(per, bind));
        /// <summary>
        /// 增加一个effect
        /// IEffect -1:团队 0-(characters.count-1):个人
        /// </summary>
        /// <param name="bind">绑定在某个其他persistent上供检测，只对出战状态和角色状态有效</param>
        /// <returns></returns>
        public void AddPersistent(ICardPersistent per, int target = -1, AbstractPersistent? bind = null)
        {
            if (target == -1)
            {
                Effects.Add(new(per, bind));
            }
            else
            {
                var cha = Characters[int.Clamp(target, 0, Characters.Length - 1)];
                if (cha.Alive)
                {
                    cha.Effects.Add(new(per, bind));
                }
            }
            Game.BroadCastRegion();
        }
        /// <summary>
        /// 自己检测满了没有，也不一定添加成功
        /// </summary>
        public void AddSupport(AbstractCardSupport support, int replace = -1)
        {
            if (Supports.Full)
            {
                Supports.TryRemoveAt(replace);
            }
            Supports.Add(new(support));
        }
        public void AddSummon(AbstractCardSummon summon)
        {
            Summons.Add(new(summon));
        }
        public void AddSummon(int num, params AbstractCardSummon[] summons)
        {
            var left = summons.Where(s => !Summons.Contains(s.GetType())).ToList();
            while (num > 0)
            {
                if (left.Count == 0)//全都召唤了，刷新
                {
                    var pool = summons.Select(p => p).ToList();
                    for (int i = 0; i < num && pool.Count > 0; i++)
                    {
                        int j = Random.Next(pool.Count);
                        Summons.Add(new(pool[j]));
                        pool.RemoveAt(j);
                    }
                    break;
                }
                else if (!Summons.Full)
                {
                    var choose = Random.Next(left.Count);
                    Summons.Add(new(left[choose]));
                    left.RemoveAt(choose);
                    num--;
                }
                else
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 注册所有角色的被动技能，通常在游戏开始出人之前
        /// </summary>
        internal void RegisterPassive()
        {
            for (int i = 0; i < Characters.Length; i++)
            {
                var c = Characters[i].Card;
                foreach (var s in c.Skills)
                {
                    if (s is AbstractCardSkillPassive ps && ps.MaxUseTimes >= 0)
                    {
                        //TODO:?
                        //AddPersistent(ps, i);
                    }
                }
            }
        }
        /// <summary>
        /// 在某一次所有的结算之后，清除not active的effect
        /// </summary>
        /// <returns>删除的effect总数量</returns>
        internal void EffectUpdate()
        {
            for (int i = 0; i < Characters.Length; i++)
            {
                Characters[(i + Characters.Length + CurrCharacter) % Characters.Length].Effects.Update();
            }
            Effects.Update();
            Summons.Update();
            Supports.Update();
        }
        /// <summary>
        /// 用于被击倒角色的受到伤害结算
        /// </summary>
        private void EffectTriggerWithoutCharacter(EventPersistentSetHandler? hs, AbstractSender sender, AbstractVariable? variable = null)
        {
            hs += Effects.GetPersistentHandlers(sender);
            hs += Summons.GetPersistentHandlers(sender);
            hs += Supports.GetPersistentHandlers(sender);
            hs?.Invoke(this, sender, variable);
            EffectUpdate();
        }
        /// <summary>
        /// effect按照 (curr->curr+1->curr+2->...)角色=>团队=>召唤物=>支援区 的顺序结算<br/>
        /// 如果不是diesender，就在结算前进行免于被击倒的结算<br/>
        /// <b>NOTE:"免于被击倒的结算"一定要在结算前消耗次数</b>
        /// </summary>
        public void EffectTrigger(AbstractSender sender, AbstractVariable? variable = null)
        {
            if (Game.InstantTrigger)
            {
                if (sender is not DieSender)
                {
                    for (int i = 0; i < Characters.Length; i++)
                    {
                        int curr = (i + CurrCharacter) % Characters.Length;
                        var cha = Characters[curr];
                        if (cha.HP == 0 && cha.Alive)
                        {
                            EffectTrigger(new DieSender(TeamIndex, curr, true));
                            if (cha.HP == 0)
                            {
                                cha.Predie = true;
                                cha.Alive = false;
                                cha.Element = 0;
                                //TODO:alive的处理
                            }
                        }
                    }
                    if (Characters.All(p => p.HP == 0))
                    {
                        throw new GameOverException();
                    }
                }

                EventPersistentSetHandler? hs = null;
                for (int i = 0; i < Characters.Length; i++)
                {
                    var c = Characters[(i + Characters.Length + CurrCharacter) % Characters.Length];
                    if (c.Alive)
                    {
                        if (c.Card.TriggerDic.TryGetValue(sender.SenderName, out var h))
                        {
                            hs += (me, s, v) => h.Trigger(me, c, s, v);
                        }
                        hs += c.Effects.GetPersistentHandlers(sender);
                    }
                }
                EffectTriggerWithoutCharacter(hs, sender, variable);
            }
            else
            {
                Game.DelayedTriggerStack.Push(() => EffectTrigger(sender, variable));
            }
        }
    }
}
