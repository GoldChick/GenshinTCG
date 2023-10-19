using System.Diagnostics;
using TCGBase;
using TCGCard;
using TCGRule;
using TCGUtil;

namespace TCGGame
{
    public abstract partial class AbstractTeam
    {
        /// <summary>
        /// TODO: 为了修改注册机制而做的<br/>
        /// 但是感觉没什么必要？就放在这里吧
        /// </summary>
        public bool AddEffect(RegistryObject<AbstractCardPersistentEffect> effect, int target = -1) => AddPersistent(effect.Value, target);
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
            if (per is AbstractCardPersistentEffect ef)
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
            else if (per is AbstractCardPersistentSupport sp)
            {
                if (!Supports.Full)
                {
                    Supports.Add(new Support(sp));
                }
                else
                {
                    Debug.Assert(target >= 0 && target < Supports.MaxSize, "AbstractTeam.Persistent.AddPersistent:场地区已满，但是未给出正确的将弃置场地index!");
                    Supports.RemoveAt(target);
                    Supports.Add(new Support(sp));
                }
            }
            else
            {
                throw new ArgumentException("AbstractTeam.AddPersistent():添加的Persistent类型不支持！");
            }
            //TODO:replace
            return false;
        }
        /// <summary>
        /// 尝试在我方场上添加单个或多个召唤物<br/>
        /// 当我方召唤物满场时，仅在provider的召唤物全在场时会进行更新
        /// </summary>
        public void TryAddSummon(IPersistentProvider<AbstractCardPersistentSummon> provider)
        {
            if (provider is ISinglePersistentProvider<AbstractCardPersistentSummon> single)
            {
                Summons.Add(new Summon(single.PersistentPool));
            }
            else if (provider is IMultiPersistentProvider<AbstractCardPersistentSummon> mul)
            {
                var left = mul.PersistentPool.Where(s => !Summons.Contains(s.NameID)).ToList();
                int num = mul.PersistentNum;
                while (num > 0)
                {
                    if (left.Count == 0)//全都召唤了，刷新
                    {
                        var pool = mul.PersistentPool.Select(p => p).ToList();
                        for (int i = 0; i < num && pool.Count > 0; i++)
                        {
                            int j = Random.Next(pool.Count);
                            Summons.Add(new Summon(pool[j]));
                            pool.RemoveAt(j);
                        }
                        break;
                    }
                    else if (!Summons.Full)
                    {
                        var choose = Random.Next(left.Count);
                        Summons.Add(new Summon(left[choose]));
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
