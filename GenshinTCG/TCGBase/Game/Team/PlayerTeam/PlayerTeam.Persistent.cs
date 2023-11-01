namespace TCGBase
{
    public partial class PlayerTeam
    {
        /// <summary>
        /// 增加一个effect
        /// IEffect -1:团队 0-(characters.count-1):个人
        /// </summary>
        /// <param name="bind">绑定在某个其他persistent上供检测，只对出战状态和角色状态有效</param>
        /// <returns></returns>
        public void AddPersistent(AbstractCardPersistentEffect per, int target = -1, AbstractPersistent? bind = null)
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
                    cha.Effects.Add(new PersonalEffect(per, bind));
                }
            }
            Game.BroadCastRegion();
        }
        /// <summary>
        /// 尝试在我方场上添加单个或多个召唤物<br/>
        /// 当我方召唤物满场时，仅在provider的召唤物全在场时会进行更新
        /// </summary>
        public void TryAddSummon(IPersistentProvider<AbstractCardPersistentSummon> provider)
        {
            if (provider is ISinglePersistentProvider<AbstractCardPersistentSummon> single)
            {
                Summons.Add(new(single.PersistentPool));
            }
            else if (provider is IMultiPersistentProvider<AbstractCardPersistentSummon> mul)
            {
                var left = mul.PersistentPool.Where(s => !Summons.Contains(s.GetType())).ToList();
                int num = mul.PersistentNum;
                while (num > 0)
                {
                    if (left.Count == 0)//全都召唤了，刷新
                    {
                        var pool = mul.PersistentPool.Select(p => p).ToList();
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

        }
        /// <summary>
        /// 注册所有角色的被动技能，通常在游戏开始出人之前
        /// </summary>
        internal void RegisterPassive()
        {
            for (int i = 0; i < Characters.Length; i++)
            {
                foreach (var s in Characters[i].Card.Skills)
                {
                    if (s is AbstractPassiveSkill ps)
                    {
                        AddPersistent(new Passive(ps, i));
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
                var c = Characters[(i + Characters.Length + CurrCharacter) % Characters.Length];
                c.Weapon.Update();
                c.Artifact.Update();
                c.Talent.Update();
                c.Effects.Update();
            }
            Effects.Update();
            Summons.Update();
            Supports.Update();
        }
        /// <summary>
        /// effect按照 (curr->curr+1->curr+2->...)角色=>团队=>召唤物=>支援区 的顺序结算
        /// </summary>
        public void EffectTrigger(Game game, int meIndex, AbstractSender sender, AbstractVariable? variable = null)
        {
            var me = game.Teams[meIndex];
            for (int i = 0; i < Characters.Length; i++)
            {
                Characters[(i + Characters.Length + CurrCharacter) % Characters.Length].EffectTrigger(me, sender, variable);
            }
            Effects.EffectTrigger(me, sender, variable);
            Summons.EffectTrigger(me, sender, variable);
            Supports.EffectTrigger(me, sender, variable);
            EffectUpdate();
        }
    }
}
