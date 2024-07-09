namespace TCGBase
{
    public partial class PlayerTeam
    {
        public void AddEffect(Persistent per, int target = -1)
        {
            if (target == -1)
            {
                Effects.Add(per);
            }
            else
            {
                Characters[int.Clamp(target, 0, Characters.Length - 1)].AddEffect(per);
            }
            Game.BroadCastRegion();
        }
        /// <summary>
        /// 增加一个effect，只在PlayerTeam中有效
        /// IEffect -1:团队 0-(characters.count-1):个人
        /// </summary>
        /// <param name="bind">绑定在某个其他persistent上供检测，只对出战状态和角色状态有效</param>
        public void AddEffect(AbstractCardBase per, int target = -1, Persistent? bind = null) => AddEffect(new Persistent(per, bind), target);
        public void AddSupport(Persistent support, int replace = -1)
        {
            if (Supports.Full)
            {
                Supports.Destroy(replace);
            }
            Supports.Add(support);
        }
        public void AddSummon(CardEffect summon)
        {
            if (summon.CardType == CardType.Summon)
            {
                Summons.Add(new(summon));
            }
        }
        public void AddSummon(int num, params Persistent[] summons)
        {
            var left = summons.Where(s => !Summons.Contains(s.CardBase) && s.CardBase.CardType == CardType.Summon).ToList();
            while (num > 0)
            {
                if (left.Count == 0)//全都召唤了，刷新
                {
                    var pool = summons.Select(p => p).ToList();
                    for (int i = 0; i < num && pool.Count > 0; i++)
                    {
                        int j = Random.Next(pool.Count);
                        Summons.Add(pool[j]);
                        pool.RemoveAt(j);
                    }
                    break;
                }
                else if (!Summons.Full)
                {
                    var choose = Random.Next(left.Count);
                    Summons.Add(left[choose]);
                    left.RemoveAt(choose);
                    num--;
                }
                else
                {
                    break;
                }
            }
        }
        public void AddSummon(int num, params CardEffect[] summons)
        {
            var left = summons.Where(s => !Summons.Contains(s) && s.CardType == CardType.Summon).ToList();
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
        internal List<EventPersistentSetHandler> GetEffectHandlers(AbstractSender sender)
        {
            List<EventPersistentSetHandler> hs = new();
            if (CurrCharacter == -1)
            {
                hs.AddRange(Effects.GetPersistentHandlers(sender));
                for (int i = 0; i < Characters.Length; i++)
                {
                    hs.AddRange(Characters[i].GetPersistentHandlers(sender));
                }
            }
            else
            {
                hs.AddRange(Characters[CurrCharacter].GetPersistentHandlers(sender));
                hs.AddRange(Effects.GetPersistentHandlers(sender));
                for (int i = 1; i < Characters.Length; i++)
                {
                    hs.AddRange(Characters[(i + CurrCharacter) % Characters.Length].GetPersistentHandlers(sender));
                }
            }
            hs.AddRange(Summons.GetPersistentHandlers(sender));
            hs.AddRange(Supports.GetPersistentHandlers(sender));
            hs.AddRange(CardsInHand.GetHandlers(sender));
            return hs;
        }
        /// <summary>
        /// 立即触发，不会储存在Queue里
        /// </summary>
        internal void InstantTrigger(AbstractSender sender, AbstractVariable? variable = null)
        {
            foreach (var handler in GetEffectHandlers(sender))
            {
                handler.Invoke(sender, variable);
            }
        }
    }
}
