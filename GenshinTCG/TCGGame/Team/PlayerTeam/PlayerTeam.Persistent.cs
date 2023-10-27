using System.Diagnostics;
using TCGBase;
using TCGMod;
using TCGRule;
using TCGUtil;

namespace TCGGame
{
    public partial class PlayerTeam
    {
        /// <summary>
        /// TODO: 为了修改注册机制而做的<br/>
        /// 但是感觉没什么必要？就放在这里吧
        /// </summary>
        //public bool AddEffect(RegistryObject<AbstractCardPersistentEffect> effect, int target = -1) => AddPersistent(effect.Value, target);

        ///<summary>
        ///添加/更新装备
        /// </summary>
        public bool AddEquipment(AbstractCardEquipment equip, int target = 0)
        {
            target = int.Clamp(target, 0, Characters.Length - 1);
            Debug.Assert(equip.CanBeUsed(this as PlayerTeam, new int[] { target }), $"{equip.NameID} Cant be Armed");
            var cha = Characters[target];
            if (equip is AbstractCardWeapon wp)
            {
                if (cha.Weapon != null)
                {
                    cha.Weapon.Active = false;
                    EffectUpdate();
                    //cha.Weapon = new PersonalEffect(wp);
                }
            }
            else if (equip is AbstractCardArtifact)
            {
            }
            //TODO:nature?
            //else if (equip is AbstractCardWeapon)
            {
            }
            return true;
        }

        /// <summary>
        /// 增加一个persistent类型的effect
        /// IEffect -1:团队 0-(characters.count-1):个人
        /// ISupport: 0-3顶掉的场地
        /// </summary>
        /// <param name="bind">绑定在某个其他persistent上供检测，只对出战状态和角色状态有效</param>
        /// <returns></returns>
        public bool AddPersistent(AbstractCardPersistent per, int target = -1, AbstractPersistent? bind = null)
        {
            if (per is AbstractCardPersistentEffect ef)
            {
                if (target == -1)
                {
                    Effects.Add(new Effect(ef, bind));
                }
                else
                {
                    int i = int.Clamp(target, 0, Characters.Length - 1);
                    var cha = Characters[i];
                    if (cha.Alive)
                    {
                        cha.Effects.Add(new PersonalEffect(ef, bind));
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
        /// 注册所有角色的被动技能，通常在游戏开始出人之前
        /// </summary>
        public void RegisterPassive()
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
        private void EffectUpdate()
        {
            Array.ForEach(Characters,c => c.Effects.Update());
            Effects.Update();
            Summons.Update();
            Supports.Update();
        }
        /// <summary>
        /// effect按照 0-N角色=>团队=>召唤物=>支援区 的顺序结算
        /// </summary>
        public void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable = null)
        {


            var me = game.Teams[meIndex];
            Array.ForEach(Characters, c => c.EffectTrigger(me, sender, variable));

            Effects.EffectTrigger(me, sender, variable);
            Summons.EffectTrigger(me, sender, variable);
            Supports.EffectTrigger(me, sender, variable);

            EffectUpdate();
            //TODO:test
            //Logger.Print($"Team{TeamIndex}清除了{EffectUpdate()}个耗尽的effect！此时的sender为{sender.SenderName}");
        }
    }
}
