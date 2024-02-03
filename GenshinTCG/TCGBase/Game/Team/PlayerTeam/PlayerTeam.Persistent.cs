namespace TCGBase
{
    public partial class PlayerTeam
    {
        /// <summary>
        /// 根据对于[出战角色]的[相对坐标]来附着[角色状态]
        /// </summary>
        public void AddPersonalEffect(AbstractCardBase per, int relativeIndex = 0, AbstractPersistent? bind = null)
        {
            Characters[(relativeIndex + CurrCharacter) % Characters.Length].AddEffect(new(per, bind));
        }
        /// <summary>
        /// 根据对于[出战角色]的[相对坐标]来附着[已经存在]的[角色状态]<br/>
        /// <b>如果没有特殊需要，请使用上面的方法创建新的persistent</b>
        /// </summary>
        public void AddPersonalEffect(Persistent<AbstractCardBase> per, int relativeIndex = 0)
        {
            Characters[(relativeIndex + CurrCharacter) % Characters.Length].AddEffect(per);
        }
        public void AddTeamEffect(AbstractCardBase per, AbstractPersistent? bind = null) => Effects.Add(new(per, bind));

        //TODO: 上面几个不知道还要不要
        public override void AddEffect(AbstractCardBase per, int target = -1, AbstractPersistent? bind = null)
        {
            if (target == -1)
            {
                Effects.Add(new(per, bind));
            }
            else
            {
                Characters[int.Clamp(target, 0, Characters.Length - 1)].AddEffect(new(per, bind));
            }
            RealGame.BroadCastRegion();
        }
        /// <summary>
        /// 自己检测满了没有，也不一定添加成功
        /// </summary>
        public override void AddSupport(AbstractCardSupport support, int replace = -1)
        {
            if (Supports.Full)
            {
                Supports.TryRemoveAt(replace);
            }
            Supports.Add(new(support));
        }
        public override void AddSummon(int num, params AbstractCardBase[] summons)
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
        internal EventPersistentSetHandler? GetCharacterEffectHandlers(AbstractSender sender)
        {
            EventPersistentSetHandler? hs = null;
            for (int i = 0; i < Characters.Length; i++)
            {
                var c = Characters[(i + Characters.Length + CurrCharacter) % Characters.Length];
                if (c.Alive)
                {
                    hs += c.GetPersistentHandlers(sender);
                }
            }
            return hs;
        }
        internal EventPersistentSetHandler? GetOtherEffectHandlers(AbstractSender sender)
        {
            EventPersistentSetHandler? hs = null;
            hs += Effects.GetPersistentHandlers(sender);
            hs += Summons.GetPersistentHandlers(sender);
            hs += Supports.GetPersistentHandlers(sender);
            return hs;
        }
        /// <summary>
        /// 用于被击倒角色的受到伤害结算
        /// </summary>
        private void EffectTriggerWithoutCharacter(EventPersistentSetHandler? hs, AbstractSender sender, AbstractVariable? variable = null)
        {
            hs += GetOtherEffectHandlers(sender);
            hs?.Invoke(this, sender, variable);
            EffectUpdate();
        }
        /// <summary>
        /// 立即触发，不会储存在Queue里
        /// </summary>
        internal virtual void InstantTrigger(AbstractSender sender, AbstractVariable? variable = null) => EffectTriggerWithoutCharacter(GetCharacterEffectHandlers(sender), sender, variable);

        public override void EffectTrigger(AbstractSender sender, AbstractVariable? variable = null)
        {
            if (RealGame.TempDelayedTriggerQueue != null)
            {
                RealGame.TempDelayedTriggerQueue.Enqueue(() => InstantTrigger(sender, variable));
            }
            else
            {
                InstantTrigger(sender, variable);
            }
        }
    }
}
