using System.Diagnostics;
using TCGBase;

namespace TCGGame
{
    public abstract partial class AbstractTeam
    {
        /// <summary>
        /// 输入的dv需要是targetRelative=false<br/>
        /// 在其中进行effect的调用
        /// </summary>
        /// <param name="d">targetRelative=false</param>
        /// <param name="overload">是否有出战角色被超载</param>
        private List<HurtSender> Hurt(out bool overload, DamageVariable d)
        {
            List<HurtSender> hss = new();
            overload = false;
            if (d.Element != -1)
            {
                EffectTrigger(Game, TeamIndex, new SimpleSender(Tags.SenderTags.ELEMENT_ENCHANT), d);
                Game.Teams[1 - TeamIndex].EffectTrigger(Game, 1 - TeamIndex, new SimpleSender(Tags.SenderTags.DAMAGE_ADD), d);
                EffectTrigger(Game, TeamIndex, new SimpleSender(Tags.SenderTags.HURT_ADD), d);
                Game.Teams[1 - TeamIndex].EffectTrigger(Game, 1 - TeamIndex, new SimpleSender(Tags.SenderTags.DAMAGE_MUL), d);
                EffectTrigger(Game, TeamIndex, new SimpleSender(Tags.SenderTags.HURT_MUL), d);
            }
            if (d.TargetExcept)
            {
                //except to non-except
                for (int i = 0; i < Characters.Length; i++)
                {
                    int index = (i + CurrCharacter) % Characters.Length;
                    if (index != d.TargetIndex)
                    {
                        DamageVariable dv_person = new(d.Source, d.Element, d.Damage, d.TargetIndex);
                        hss.AddRange(Hurt(out bool one_overload, dv_person));
                        overload = overload || one_overload;
                    }
                }
            }
            else
            {
                //only one target
                string? reaction = GetReaction(d, out DamageVariable? mul);
                overload = reaction == Tags.ReactionTags.OVERLOADED && d.TargetIndex == CurrCharacter;
                hss.Add(new(d, reaction));
                if (mul != null)
                {
                    hss.AddRange(Hurt(out bool one_overload, mul));
                    overload = overload || one_overload;
                }
            }
            return hss;
        }
        /// <returns>经过merge的hurtsender们</returns>
        private List<HurtSender> MultiHurt(out bool overload, params DamageVariable[] dvs)
        {
            overload = false;
            List<HurtSender> hss = new();
            foreach (var item in dvs)
            {
                hss.AddRange(Hurt(out bool one_overload, item));
                overload = overload || one_overload;
            }

            List<HurtSender> selects = new();
            foreach (var hs in hss)
            {
                var hurt = selects.Find(h => h.TargetIndex == hs.TargetIndex && h.Element == hs.Element);
                if (hurt != null)
                {
                    hurt.Damage += hs.Damage;
                }
                else
                {
                    selects.Add(hs);
                }
            }
            return selects;
        }
        private void CheckDie()
        {
            for (int i = 0; i < Characters.Length; i++)
            {
                int curr=(i+CurrCharacter)
            }
            
        }
        private void CheckDie(int curr)
        {
            Character target = Characters[curr];
            if (target.HP == 0)
            {
                EffectTrigger(Game, TeamIndex, new DieSender(curr, true), null);
                if (target.HP == 0)
                {
                    EffectTrigger(Game, TeamIndex, new DieSender(curr), null);
                    target.Alive = false;
                    //TODO:掉装备
                    if (Characters.All(p => !p.Alive))
                    {
                        //TODO:全死了之后如何结束  
                        throw new Exception("所有角色都死亡了，游戏结束！");
                    }
                    Game.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced, "Character Died");
                }
            }
        }
        /// <param name="action">伤害结算后，死亡结算前结算的东西，如[风压剑]</param>
        public void MultiHurt(DamageVariable[] dvs, Action? action = null)
        {
            DamageVariable[] dvs_person = dvs.Select(p => p.TargetRelative ? new(p.Source, p.Element, p.Damage, (p.TargetIndex + CurrCharacter) % Characters.Length) : p).ToArray();
            List<HurtSender> hss = MultiHurt(out bool overload, dvs_person);
            foreach (var hs in hss)
            {
                Characters[hs.TargetIndex].HP -= hs.Damage;
            }
            action?.Invoke();
            if (overload)
            {
                SwitchToNext();
            }
            foreach (var hs in hss)
            {
                Game.EffectTrigger(hs, null);
            }
        }
        /// <param name="action">伤害结算后，死亡结算前结算的东西</param>
        public void Hurt(DamageVariable dv, Action? action = null)
        {
            Debug.Assert(dv.TargetRelative, "AbstractTeam.Hurt():输入的dv(直接来源于角色技能、卡牌等)的targetRelative为false!");
            Character target;
            int curr = CurrCharacter;
            do
            {
                curr = (curr + dv.TargetIndex) % Characters.Length;
                target = Characters[curr];
            } while (!target.Alive);
            DamageVariable d = new(dv.Source, dv.Element, dv.Damage, curr, dv.TargetExcept);
            if (d.Element != -1)
            {
                EffectTrigger(Game, TeamIndex, new SimpleSender(Tags.SenderTags.ELEMENT_ENCHANT), d);
                Game.Teams[1 - TeamIndex].EffectTrigger(Game, 1 - TeamIndex, new SimpleSender(Tags.SenderTags.DAMAGE_ADD), d);
                EffectTrigger(Game, TeamIndex, new SimpleSender(Tags.SenderTags.HURT_ADD), d);
                Game.Teams[1 - TeamIndex].EffectTrigger(Game, 1 - TeamIndex, new SimpleSender(Tags.SenderTags.DAMAGE_MUL), d);
                EffectTrigger(Game, TeamIndex, new SimpleSender(Tags.SenderTags.HURT_MUL), d);
            }
            string? reaction = GetReaction(d, out DamageVariable? mul);

            target.HP -= d.Damage;

            action?.Invoke();

        }
        public void Hurt(int element, int baseDamage, DamageSource source, int relativeIndex)
        {
            if (Characters.All(c => !c.Alive))
            {
                throw new Exception($"AbstractTeam.Hurt(): All Characters Died");
            }
            Character target;
            int curr = CurrCharacter;
            do
            {
                curr = (curr + relativeIndex) % Characters.Length;
                target = Characters[curr];
            } while (!target.Alive);

            //element damage经过normalize
            DamageVariable d = new(element, baseDamage, source, curr);

            EffectTrigger(Game, TeamIndex, new SimpleSender(Tags.SenderTags.ELEMENT_ENCHANT), d);
            Game.Teams[1 - TeamIndex].EffectTrigger(Game, 1 - TeamIndex, new SimpleSender(Tags.SenderTags.DAMAGE_ADD), d);
            EffectTrigger(Game, TeamIndex, new SimpleSender(Tags.SenderTags.HURT_ADD), d);
            Game.Teams[1 - TeamIndex].EffectTrigger(Game, 1 - TeamIndex, new SimpleSender(Tags.SenderTags.DAMAGE_MUL), d);
            EffectTrigger(Game, TeamIndex, new SimpleSender(Tags.SenderTags.HURT_MUL), d);
            //TODO:穿透无增伤
            target.HP -= d.Damage;
            //TODO:元素反应
            if (target.HP == 0)
            {
                EffectTrigger(Game, TeamIndex, new DieSender(curr, true), null);
                if (target.HP == 0)
                {
                    EffectTrigger(Game, TeamIndex, new DieSender(curr), null);
                    target.Alive = false;
                    //TODO:掉装备
                    //TODO:全死了之后如何结束  
                    //TODO:选择时间
                    Game.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced, "Character Died");
                }
            }
        }
        ///<summary>
        /// mul : targetRelative=false;<br/>
        /// 注意：调用此方法将改变角色头上的元素!
        /// </summary>
        /// <returns></returns>
        public string? GetReaction(DamageVariable dv_person, out DamageVariable? mul)
        {
            //角色身上附着的元素(只允许附着 无0 冰1 水2 火3 雷4 草6 <b>冰+草5</b>
            mul = null;

            string? reaction = null;
            int currElement = Characters[dv_person.TargetIndex].Element;
            int nextElement = currElement;

            if (dv_person.Element > 0)
            {
                nextElement = 0;

                int reactionType = dv_person.Element * 10 + currElement;
                switch (reactionType)
                {
                    case 12 or 21://冻结
                    case 25:
                        reaction = Tags.ReactionTags.FROZEN;
                        dv_person.Damage++;
                        break;

                    case 13 or 31://融化
                    case 35:
                        reaction = Tags.ReactionTags.MELT;
                        dv_person.Damage += 2;
                        break;

                    case 14 or 41://超导
                    case 45:
                        reaction = Tags.ReactionTags.SUPERCONDUCT;
                        dv_person.Damage++;
                        mul = new(DamageSource.NoWhere, -1, 1, dv_person.TargetIndex, true);
                        break;

                    case 23 or 32://蒸发
                        reaction = Tags.ReactionTags.VAPORIZE;
                        dv_person.Damage += 2;
                        break;

                    case 24 or 42://感电
                        reaction = Tags.ReactionTags.ELECTRO_CHARGED;
                        dv_person.Damage++;
                        mul = new(DamageSource.NoWhere, -1, 1, dv_person.TargetIndex, true);
                        break;

                    case 34 or 43://超载
                        reaction = Tags.ReactionTags.OVERLOADED;
                        dv_person.Damage += 2;
                        break;

                    case 51 or 52 or 53 or 54://结晶
                    case 55:
                        reaction = Tags.ReactionTags.CRYSTALLIZE;
                        dv_person.Damage++;
                        break;

                    //NOTE:冰草共存优先反应冰

                    case 62 or 26://绽放
                        reaction = Tags.ReactionTags.BLOOM;
                        dv_person.Damage++;
                        break;

                    case 63 or 36://燃烧
                        reaction = Tags.ReactionTags.BURNING;
                        dv_person.Damage++;
                        break;

                    case 64 or 46://激化
                        reaction = Tags.ReactionTags.CATALYZE;
                        dv_person.Damage++;
                        break;

                    case 71 or 72 or 73 or 74://扩散
                    case 75:
                        reaction = Tags.ReactionTags.SWIRL;
                        mul = new(DamageSource.NoWhere, (currElement - 1) % 4 + 1, 1, dv_person.TargetIndex, true);
                        break;

                    case 61 or 16://不反应，但是冰草共存
                        nextElement = 5;
                        break;

                    case 10 or 20 or 30 or 40://不反应，但是改变附着
                        nextElement = dv_person.Element;
                        break;

                    default://不反应，也不改变附着
                        nextElement = currElement;
                        break;
                }

                //冰草共存检测是否反应掉了冰
                if (currElement == 5 && reaction != null)
                {
                    nextElement = 6;
                }
            }

            Characters[dv_person.TargetIndex].Element = nextElement;

            return reaction;
        }
    }
}
