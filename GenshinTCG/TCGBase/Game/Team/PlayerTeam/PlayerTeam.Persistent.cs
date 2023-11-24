﻿using System.Runtime.InteropServices;

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
        public void AddSummon(AbstractCardPersistentSummon summon)
        {
            Summons.Add(new(summon));
        }
        public void AddSummon(int num, params AbstractCardPersistentSummon[] summons)
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
                        AddPersistent(ps, i);
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
        /// effect按照 (curr->curr+1->curr+2->...)角色=>团队=>召唤物=>支援区 的顺序结算
        /// </summary>
        public void EffectTrigger(AbstractSender sender, AbstractVariable? variable = null, bool update = true)
        {
            EventPersistentSetHandler? hs = null;
            for (int i = 0; i < Characters.Length; i++)
            {
                hs += Characters[(i + Characters.Length + CurrCharacter) % Characters.Length].Effects.GetPersistentHandlers(sender);
            }
            hs += Effects.GetPersistentHandlers(sender);
            hs += Summons.GetPersistentHandlers(sender);
            hs += Supports.GetPersistentHandlers(sender);
            hs?.Invoke(this, sender, variable);
            
            if (update)
            {
                EffectUpdate();
            }
        }
    }
}
