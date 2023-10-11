using TCGBase;
using TCGCard;
using TCGRule;
using TCGUtil;

namespace TCGGame
{
    public abstract partial class AbstractTeam
    {
        public bool AddEffect(RegistryObject<AbstractCardEffect> effect, int target = -1) => AddPersistent(effect.Value, target);
        /// <summary>
        /// 增加一个persistent类型的effect
        /// IEffect -1:团队 0-(characters.count-1):个人
        /// ISupport: 0-3顶掉的场地
        /// </summary>
        /// <param name="per"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool AddPersistent(AbstractCardPersistent per, int target = -1)
        {
            if (per is AbstractCardEffect ef)
            {
                if (target == -1)
                {
                    Effects.Add(new Effect(ef));
                }
                else
                {
                    int i = int.Clamp(target, 0, Characters.Length - 1);
                    var cha = Characters[i];
                    if (cha.Alive)
                    {
                        cha.Effects.Add(new PersonalEffect(i, ef));
                    }
                    else
                    {
                        Logger.Warning($"AbstractTeam.AddPersistent():添加名为{ef.NameID}的effect时出现问题：角色已经被击倒！");
                    }
                }
            }
            else if (per is AbstractCardSupport sp)
            {
                //TODO:场地弃置
                if (!Supports.Full)
                {
                    Supports.Add(new Support(sp));
                }
            }
            else if (per is AbstractCardSummon sm)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new ArgumentException("AbstractTeam.AddPersistent():添加的Persistent类型不支持！");
            }
            //TODO:replace
            return false;
        }
        //TODO:找到地方调用这个东西

        public void TryAddSummon(ISummonProvider provider)
        {
            var left = provider.PersistentPool.Where(s => !Summons.Contains(s.NameID)).ToList();
            if (left.Count == 0)//全都召唤了，刷新
            {
                var pool = provider.PersistentPool.Select(p => p).ToList();
                if (provider.PersistentOrdered)
                {
                    for (int i = 0; i < int.Min(provider.PersistentNum, pool.Count); i++)
                    {
                        Summons.Add(new Summon(pool[i]));
                    }
                }
                else
                {
                    for (int i = 0; i < provider.PersistentNum && pool.Count > 0; i++)
                    {
                        int j = Random.Next(pool.Count);
                        pool.RemoveAt(j);
                        Summons.Add(new Summon(pool[j]));
                    }
                }
            }
            else if (!Summons.Full)
            {
                Summons.Add(new Summon(left[Random.Next(left.Count)]));
            }
        }
        /// <summary>
        /// 在某一次所有的结算之后，清除not active的effect
        /// </summary>
        /// <returns>删除的effect总数量</returns>
        private int EffectUpdate()
        {
            int sum = Characters.Select(c => c.Effects.Update()).Sum();
            sum += Effects.Update();
            sum += Summons.Update();
            sum += Supports.Update();
            return sum;
        }
        /// <summary>
        /// effect按照 0-N角色=>团队=>召唤物=>支援区 的顺序结算
        /// </summary>
        public void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable = null)
        {
            Array.ForEach(Characters, c => c.EffectTrigger(game, meIndex, sender, variable));

            Effects.EffectTrigger(game, meIndex, sender, variable);
            Summons.EffectTrigger(game, meIndex, sender, variable);
            Supports.EffectTrigger(game, meIndex, sender, variable);

            EffectUpdate();
            //TODO:test
            //Logger.Print($"Team{TeamIndex}清除了{EffectUpdate()}个耗尽的effect！此时的sender为{sender.SenderName}");
        }
    }
}
