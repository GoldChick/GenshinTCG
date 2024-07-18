namespace TCGBase
{
    public partial class Game
    {
        /// <summary>
        /// 输入的dv满足targetRelative=false<br/>
        /// 在其中进行effect的调用
        /// </summary>
        private List<DamageVariable> InnerHurtCompute(HurtSourceSender sourceSender, DamageRecord start, out Action? reactionAction)
        {
            List<DamageVariable> list = new();
            reactionAction = null;

            DamageRecord? curr = start;
            do
            {
                var currteamid = 1 - sourceSender.TeamID ^ (int)curr.Team;
                var currteam = Teams[currteamid];
                if (curr.TargetArea == TargetArea.TargetOnly)
                {
                    list.Add(new DamageVariable(currteamid, curr.Element, curr.Amount, DamageSource.Direct, ((currteam.CurrCharacter + curr.TargetIndexOffset) % currteam.Characters.Count + currteam.Characters.Count) % currteam.Characters.Count));
                }
                else
                {
                    if (currteam.CurrCharacter == -1)
                    {
                        throw new ArgumentOutOfRangeException(nameof(start), "InnerHurtCompute:怎么有队伍的CurrCharacter是-1啊？");
                    }
                    list.AddRange(Enumerable.Range(0, currteam.Characters.Count)
                        .OrderBy(i => ((i - currteam.CurrCharacter - curr.TargetIndexOffset) % currteam.Characters.Count + currteam.Characters.Count) % currteam.Characters.Count)
                        .Skip(1).Where(i => currteam.Characters[i].Alive)
                        .Select(i => new DamageVariable(currteamid, curr.Element, curr.Amount, DamageSource.Direct, i)));
                }
                curr = curr.SubDamage;
            } while (curr != null);

            for (int i = 0; i < list.Count; i++)
            {
                DamageVariable dv = list[i];
                if (dv.Element is not DamageElement.Pierce)
                {
                    InstantTrigger(SenderTag.ElementEnchant.ToString(), sourceSender, dv, instant: true);
                    //element reaction subdamage here ↓
                    list.AddRange(GetDamageReaction(dv));

                    InstantTrigger(SenderTag.DamageIncrease.ToString(), sourceSender, dv, instant: true);

                    dv.Amount = (int)(dv.Amount * dv.Mul);

                    Teams[dv.TargetTeam].InstantTrigger(SenderTag.HurtDecrease.ToString(), sourceSender, dv);
                    //TODO: element modifier
                    var ac = ReactionActionGenerate(dv);
                    //InstantTrigger(xxx)
                    reactionAction += () => ac?.Invoke();
                }
            }
            return list;
        }

        /// <summary>
        /// specialAction:特殊效果，触发在反应效果后，免于被击倒前；会放到queue里<br/>
        /// </summary>
        internal void InnerHurt(DamageRecord? damage, HurtSourceSender sourceSender, Action? specialAction = null)
        {
            List<DamageVariable> valid_dvs = new();

            Action? todie_or_nottodie = null;
            if (damage != null)
            {
                DelayedTriggerQueue.TryTrigger(() =>
                {
                    if (Teams.Any(t => t.Characters.All(c => !c.Alive)))
                    {
                        throw new GameOverException();
                    }
                });
                var dvs = InnerHurtCompute(sourceSender, damage, out Action? reactionAction);

                foreach (var dv in dvs)
                {
                    var currteam = Teams[dv.TargetTeam];
                    var cha = currteam.Characters[dv.TargetIndex];
                    if (cha.HP > 0)
                    {
                        valid_dvs.Add(dv);
                        cha.HP -= dv.Amount;
                        if (cha.HP == 0)
                        {
                            if (cha.Effects.TryFind(e => e.CardBase.TriggerableList.ContainsKey(SenderTag.AntiDie.ToString()), out var p))
                            {
                                todie_or_nottodie += () =>
                                {
                                    foreach (var it in p.CardBase.TriggerableList)
                                    {
                                        if (it.Tag == SenderTag.AntiDie.ToString())
                                        {
                                            it.Trigger(currteam, p, new SimpleSender(), null);
                                        }
                                    }
                                };
                            }
                            else
                            {
                                cha.ToDieLimbo();
                                todie_or_nottodie += cha.ToDie;
                            }
                        }
                    }
                    BroadCast(ClientUpdateCreate.CharacterUpdate.HurtUpdate(currteam.TeamID, dv.TargetIndex, dv.Element, dv.Amount));
                }

                reactionAction?.Invoke();
            }
            specialAction?.Invoke();
            todie_or_nottodie?.Invoke();

            var alive_dvs = valid_dvs.Where(dv => Teams[dv.TargetTeam].Characters[dv.TargetIndex].Alive);
            var died_dvs = valid_dvs.Where(dv => !Teams[dv.TargetTeam].Characters[dv.TargetIndex].Alive).DistinctBy(dv => dv.TargetIndex);

            DelayedTriggerQueue.TryTrigger(() =>
            {
                foreach (var dv in alive_dvs)
                {
                    EffectTrigger(SenderTag.AfterHurt.ToString(), sourceSender, dv);
                }
                foreach (var dv in died_dvs)
                {
                    dv.Deadly = true;
                    Teams[dv.TargetTeam].Characters[dv.TargetIndex].DieTrigger(sourceSender, dv);
                    NetEventRecords.Last().Add(new DieRecord(dv.TargetTeam, Teams[dv.TargetTeam].Characters[dv.TargetIndex]));
                    EffectTrigger(SenderTag.AfterHurt.ToString(), sourceSender, dv);
                }
                var dieTeams = Teams.Where(t => !t.Characters[t.CurrCharacter].Alive);
                if (dieTeams.Count() == 2)
                {
                    throw new Exception("TODO:共死其实还没做.....");
                    ////双方出战角色都被击倒 进入选择角色出战
                    //var t0 = new Task(() => RealGame.RequestAndHandleEvent(TeamIndex, 30000, ActionType.SwitchForced));
                    //var t1 = new Task(() => RealGame.RequestAndHandleEvent(1 - TeamIndex, 30000, ActionType.SwitchForced));
                    //t0.Start();
                    //t1.Start();
                    //Task.WaitAll(t0, t1);
                    ////TODO:其实不该执行，应该都选择出战之后才能执行...
                }
                else if (dieTeams.Count() == 1)
                {
                    RequestAndHandleEvent(dieTeams.ElementAt(0).TeamID, 30000, OperationType.Switch);
                }
            });
        }
    }
}
