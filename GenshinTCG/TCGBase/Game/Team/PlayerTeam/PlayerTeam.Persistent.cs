namespace TCGBase
{
    public partial class PlayerTeam
    {
        /// <summary>
        /// 根据对于[出战角色]的[相对坐标]来附着[角色状态]
        /// </summary>
        public void AddPersonalEffect(AbstractCardBase per, int relativeIndex = 0, Persistent? bind = null)
        {
            Characters[(relativeIndex + CurrCharacter) % Characters.Length].AddEffect(new Persistent(per, bind));
        }
        /// <summary>
        /// 根据对于[出战角色]的[相对坐标]来附着[已经存在]的[角色状态]<br/>
        /// <b>如果没有特殊需要，请使用上面的方法创建新的persistent</b>
        /// </summary>
        public void AddPersonalEffect(Persistent per, int relativeIndex = 0)
        {
            Characters[(relativeIndex + CurrCharacter) % Characters.Length].AddEffect(per);
        }
        public void AddTeamEffect(AbstractCardBase per, Persistent? bind = null) => Effects.Add(new Persistent(per, bind));

        //TODO: 上面几个不知道还要不要
        /// <summary>
        /// 增加一个effect，只在PlayerTeam中有效
        /// IEffect -1:团队 0-(characters.count-1):个人
        /// </summary>
        /// <param name="bind">绑定在某个其他persistent上供检测，只对出战状态和角色状态有效</param>
        public void AddEffect(AbstractCardBase per, int target = -1, Persistent? bind = null)
        {
            if (target == -1)
            {
                Effects.Add(new(per, bind));
            }
            else
            {
                Characters[int.Clamp(target, 0, Characters.Length - 1)].AddEffect(new Persistent(per, bind));
            }
            Game.BroadCastRegion();
        }
        public void AddSupport(Persistent support, int replace = -1)
        {
            if (Supports.Full)
            {
                Supports.Destroy(replace);
            }
            Supports.Add(support);
        }
        /// <summary>
        /// 自己检测满了没有，也不一定添加成功
        /// </summary>
        public void AddSupport(AbstractCardBase support, int replace = -1)
        {
            if (Supports.Full)
            {
                Supports.Destroy(replace);
            }
            Supports.Add(new(support));
        }
        public void AddSummon(AbstractCardBase summon) => AddSummon(1, summon);
        public void AddSummon(int num, params AbstractCardBase[] summons)
        {
            var left = summons.Where(s => !Summons.Contains(s.GetType()) && s.CardType == CardType.Summon).ToList();
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
        internal EventPersistentSetHandler? GetEffectHandlers(AbstractSender sender)
        {
            EventPersistentSetHandler? hs = null;
            if (CurrCharacter == -1)
            {
                hs += Effects.GetPersistentHandlers(sender);
                for (int i = 0; i < Characters.Length; i++)
                {
                    hs += Characters[i].GetPersistentHandlers(sender);
                }
            }
            else
            {
                hs += Characters[CurrCharacter].GetPersistentHandlers(sender);
                hs += Effects.GetPersistentHandlers(sender);
                for (int i = 1; i < Characters.Length; i++)
                {
                    hs += Characters[(i + CurrCharacter) % Characters.Length].GetPersistentHandlers(sender);
                }
            }
            hs += Summons.GetPersistentHandlers(sender);
            hs += Supports.GetPersistentHandlers(sender);
            hs += CardsInHand.GetHandlers(sender);
            return hs;
        }
        /// <summary>
        /// 立即触发，不会储存在Queue里
        /// </summary>
        internal void InstantTrigger(AbstractSender sender, AbstractVariable? variable = null)
        {
            GetEffectHandlers(sender)?.Invoke(this, sender, variable);
            if (CurrCharacter == -1)
            {
                Effects.Update();
                for (int i = 0; i < Characters.Length; i++)
                {
                    Characters[i].Effects.Update();
                }
            }
            else
            {
                Characters[CurrCharacter].Effects.Update();
                Effects.Update();
                for (int i = 1; i < Characters.Length; i++)
                {
                    Characters[(i + CurrCharacter) % Characters.Length].Effects.Update();
                }
            }
            Summons.Update();
            Supports.Update();
        }
        /// <summary>
        /// effect按照 (curr)角色=>团队=>(curr+1->curr+2->...)角色=>召唤物=>支援区 的顺序结算<br/>
        /// </summary>
        public void EffectTrigger(AbstractSender sender, AbstractVariable? variable = null)
        {
            if (Game.TempDelayedTriggerQueue != null)
            {
                Game.TempDelayedTriggerQueue.Enqueue(() => InstantTrigger(sender, variable));
            }
            else
            {
                InstantTrigger(sender, variable);
            }
        }
    }
}
