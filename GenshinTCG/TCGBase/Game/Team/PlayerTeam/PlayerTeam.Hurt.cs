namespace TCGBase
{
    public partial class PlayerTeam
    {
        /// <summary>
        /// 输入的dv需要是targetRelative=false<br/>
        /// 在其中进行effect的调用
        /// </summary>
        /// <param name="d">targetRelative=false</param>
        /// <param name="overload">是否有出战角色被超载</param>
        private List<HurtSender> Hurt(IDamageSource ds, out bool overload, DamageVariable d)
        {
            List<HurtSender> hss = new();
            overload = false;
            if (d.TargetExcept)
            {
                //except to non-except
                for (int i = 0; i < Characters.Length; i++)
                {
                    int index = (i + CurrCharacter) % Characters.Length;
                    if (index != d.TargetIndex)
                    {
                        DamageVariable dv_person = new(d.DirectSource, d.Element, d.Damage, index);
                        hss.AddRange(Hurt(ds, out bool one_overload, dv_person));
                        overload = overload || one_overload;
                    }
                }
            }
            else
            {
                //only one target
                Game.EffectTrigger(new PreHurSender(1 - TeamIndex, ds, SenderTag.ElementEnchant.ToString()), d);
                string? reaction = GetReaction(d, out DamageVariable? mul);
                if (d.Element != -1)
                {
                    Game.EffectTrigger(new PreHurSender(1 - TeamIndex, ds, SenderTag.DamageIncrease.ToString()), d);
                    Game.EffectTrigger(new PreHurSender(TeamIndex, ds, SenderTag.HurtDecrease.ToString()), d);
                    Game.EffectTrigger(new PreHurSender(1 - TeamIndex, ds, SenderTag.DamageMul.ToString()), d);
                    Game.EffectTrigger(new PreHurSender(TeamIndex, ds, SenderTag.HurtMul.ToString()), d);
                }

                hss.Add(new(TeamIndex, d, reaction));

                if (reaction == ReactionTags.Bloom.ToString())
                {
                    Enemy.AddPersistent(new DendroCore());
                }
                else if (reaction == ReactionTags.Burning.ToString())
                {
                    Enemy.TryAddSummon(new Burning());
                }
                else if (reaction == ReactionTags.Catalyze.ToString())
                {
                    Enemy.AddPersistent(new CatalyzeField());

                }
                else if (reaction == ReactionTags.Crystallize.ToString())
                {
                    Enemy.AddPersistent(new Crystal());
                }
                else if (reaction == ReactionTags.Overloaded.ToString())
                {
                    overload = d.TargetIndex == CurrCharacter;
                }
                else if (reaction == ReactionTags.Frozen.ToString())
                {
                    //TODO:frozen?
                }

                if (mul != null)
                {
                    hss.AddRange(Hurt(ds, out bool one_overload, mul));
                    overload = overload || one_overload;
                }
            }
            return hss;
        }
        /// <returns>经过merge的hurtsender们</returns>
        private List<HurtSender> MultiHurt(IDamageSource ds, out bool overload, params DamageVariable[] dvs)
        {
            overload = false;
            List<HurtSender> hss = new();
            foreach (var item in dvs)
            {
                hss.AddRange(Hurt(ds, out bool one_overload, item));
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
        /// <summary>
        /// 确定死亡的角色
        /// </summary>
        private void CheckDie()
        {
            for (int i = 0; i < Characters.Length; i++)
            {
                int curr = (i + CurrCharacter) % Characters.Length;
                Character target = Characters[curr];
                if (target.Predie)
                {
                    EffectTrigger(Game, TeamIndex, new DieSender(TeamIndex, curr), null);
                    target.Predie = false;
                    target.Alive = false;

                    target.Weapon = null;
                    target.Artifact = null;
                    target.Talent = null;
                    target.MP=0;
                    target.Effects.Clear();
                }
            }
            if (Characters.All(p => !p.Alive))
            {
                throw new GameOverException();
            }
            if (!Characters[CurrCharacter].Alive)
            {
                Game.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced, "Character Died");
            }
        }
        /// <param name="action">伤害结算后，死亡结算前结算的东西，如[风压剑]</param>
        public void MultiHurt(DamageVariable[] dvs, IDamageSource ds, Action? action = null)
        {
            //assert dvs.all(p=>p.targetrelative)
            DamageVariable[] dvs_person = dvs.Select(p => new DamageVariable(ds.DamageSource, p.Element, p.Damage, (p.TargetIndex + CurrCharacter) % Characters.Length, p.TargetExcept)).ToArray();
            List<HurtSender> hss = MultiHurt(ds, out bool overload, dvs_person);
            foreach (var hs in hss)
            {
                Characters[hs.TargetIndex].HP -= hs.Damage;
            }
            for (int i = 0; i < Characters.Length; i++)
            {
                int curr = (i + CurrCharacter) % Characters.Length;
                var cha = Characters[curr];
                if (cha.HP == 0 && cha.Alive && !cha.Predie)
                {
                    EffectTrigger(Game, TeamIndex, new DieSender(TeamIndex, curr, true), null);
                    if (cha.HP == 0)
                    {
                        cha.Predie = true;
                    }
                }
            }
            if (Characters.All(p => p.HP == 0))
            {
                throw new GameOverException();
            }
            //判断共死
            if (Characters[CurrCharacter].HP == 0 && Enemy.Characters[Enemy.CurrCharacter].HP == 0)
            {
                //双方出战角色都被击倒 进入选择角色出战
                var t0 = new Task(() => Game.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced, "Die Together"));
                var t1 = new Task(() => Game.RequestAndHandleEvent(1 - TeamIndex, 30000, ActionType.SwitchForced, "Die Together"));
                t0.Start();
                t1.Start();
                Task.WaitAll(t0, t1);
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
            CheckDie();
        }
        /// <param name="action">伤害结算后，死亡结算前结算的东西</param>
        public void Hurt(DamageVariable dv, IDamageSource ds, Action? action = null) => MultiHurt(new DamageVariable[] { dv }, ds, action);
        ///<summary>
        /// mul : targetRelative=false;<br/>
        /// 注意：调用此方法将改变角色头上的元素!
        /// </summary>
        public string? GetReaction(DamageVariable dv_person, out DamageVariable? mul)
        {
            //角色身上附着的元素(只允许附着 无0 冰1 水2 火3 雷4 草6 <b>冰+草5</b>
            mul = null;

            ReactionTags reactiontag = ReactionTags.None;

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
                        reactiontag = ReactionTags.Frozen;
                        dv_person.Damage++;
                        break;

                    case 13 or 31://融化
                    case 35:
                        reactiontag = ReactionTags.Melt;
                        dv_person.Damage += 2;
                        break;

                    case 14 or 41://超导
                    case 45:
                        reactiontag = ReactionTags.SuperConduct;
                        dv_person.Damage++;
                        mul = new(DamageSource.NoWhere, -1, 1, dv_person.TargetIndex, true);
                        break;

                    case 23 or 32://蒸发
                        reactiontag = ReactionTags.Vaporize;
                        dv_person.Damage += 2;
                        break;

                    case 24 or 42://感电
                        reactiontag = ReactionTags.ElectroCharged;
                        dv_person.Damage++;
                        mul = new(DamageSource.NoWhere, -1, 1, dv_person.TargetIndex, true);
                        break;

                    case 34 or 43://超载
                        reactiontag = ReactionTags.Overloaded;
                        dv_person.Damage += 2;
                        break;

                    case 51 or 52 or 53 or 54://结晶
                    case 55:
                        reactiontag = ReactionTags.Crystallize;
                        dv_person.Damage++;
                        break;

                    //NOTE:冰草共存优先反应冰

                    case 62 or 26://绽放
                        reactiontag = ReactionTags.Bloom;
                        dv_person.Damage++;
                        break;

                    case 63 or 36://燃烧
                        reactiontag = ReactionTags.Burning;
                        dv_person.Damage++;
                        break;

                    case 64 or 46://激化
                        reactiontag = ReactionTags.Catalyze;
                        dv_person.Damage++;
                        break;

                    case 71 or 72 or 73 or 74://扩散
                    case 75:
                        reactiontag = ReactionTags.Swirl;
                        mul = new(DamageSource.Addition, (currElement - 1) % 4 + 1, 1, dv_person.TargetIndex, true);
                        break;

                    case 61 or 16://不反应，但是冰草共存
                        nextElement = 5;
                        break;

                    case 10 or 20 or 30 or 40 or 60://不反应，但是改变附着
                        nextElement = dv_person.Element;
                        break;

                    default://不反应，也不改变附着
                        nextElement = currElement;
                        break;
                }

                //冰草共存检测是否反应掉了冰
                if (currElement == 5 && reactiontag != ReactionTags.None)
                {
                    nextElement = 6;
                }
            }

            Characters[dv_person.TargetIndex].Element = nextElement;

            dv_person.Reaction = reactiontag.ToString();

            return reactiontag.ToString();
        }
    }
}
