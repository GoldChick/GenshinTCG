﻿using System.Diagnostics;
using System.Text.Json;
using TCGBase;
using TCGUtil;

namespace TCGGame
{
    public abstract partial class AbstractGame
    {
        /// <summary>
        /// 向对应客户端请求一个valid的事件，或者default的事件
        /// </summary>
        /// <returns>返回的必然是有效事件</returns>
        public NetEvent RequestEvent(int teamid, int millisecondsTimeout, ActionType demand, string help_txt = "Null")
        {
            CancellationTokenSource ts = new();
            var t = new Task<NetEvent>(() => Clients[teamid].RequestEvent(demand, help_txt), ts.Token);
            t.Start();
            Task.WaitAny(t, Task.Run(() => Thread.Sleep(millisecondsTimeout)));
            if (t.IsCompleted)
            {
                if (Teams[teamid].IsEventValid(t.Result))
                {
                    //Logger.Warning($"AbstractGame.RequestEvent()::{JsonSerializer.Serialize(Teams[CurrTeam].GetEventRequirement(t.Result.Action))}");
                    return t.Result;
                }
                //Logger.Warning("AbstractGame.RequestEvent():无效的NetEvent!");
            }
            Logger.Warning($"AbstractGame.RequestEvent():请求NetEvent超时或者无效，使用默认值！");
            ts.Cancel();

            switch (demand)
            {
                case ActionType.ReRollCard:
                    return new NetEvent(new NetAction(demand), new int[8], Enumerable.Repeat(0, Teams[teamid].CardsInHand.Count).ToArray());
                case ActionType.ReRollDice:
                    return new NetEvent(new NetAction(demand), new int[8], Enumerable.Repeat(0, Teams[teamid].GetDices().Sum()).ToArray());
                case ActionType.SwitchForced:
                    {
                        var chas = Teams[teamid].Characters;
                        for (int i = 0; i < chas.Length; i++)
                        {
                            if (chas[i].Alive)
                            {
                                return new NetEvent(new NetAction(demand, i));
                            }
                        }
                        throw new Exception("AbstractGame.NetEvent.RequestEvent():demand=SwitchForced时出现错误！角色全部死亡！");
                    }
                default:
                    return new NetEvent(new NetAction(ActionType.Pass));
            }
        }
        public void RequestAndHandleEvent(int teamid, int millisecondsTimeout, ActionType demand, string help_txt = "Null")
          => HandleEvent(RequestEvent(teamid, millisecondsTimeout, demand, help_txt), teamid);
        ///<summary>
        /// 这里处理的事件全都是已经确定valid的
        /// 所以不会进行任何检测
        /// </summary>
        /// <param name="evt">已经证明是valid的NetEvent</param>
        /// <returns>是否是战斗行动</returns>
        public virtual void HandleEvent(NetEvent evt, int currTeam)
        {
            var t = Teams[currTeam];

            //before_xx 通知要发生一个xx事件，[常九爷]等可以开始检测，检测到after_xx之后判定
            EffectTrigger(new SimpleSender(currTeam, evt.Action.Type.ToSenderTags(true)));
            //cost
            t.CostDices(evt.CostArgs);

            AbstractSender afterEventSender = new SimpleSender(currTeam, evt.Action.Type.ToSenderTags());
            FastActionVariable? afterEventVariable = null;

            switch (evt.Action.Type)
            {
                case ActionType.ReRollDice:
                    var dices = Teams[currTeam].Dices;
                    int cnt = dices.Count;
                    var dicecash = dices.Where((value, index) => evt.AdditionalTargetArgs[index] == 0);
                    dices.Clear();
                    dices.AddRange(dicecash);
                    DiceRollingVariable dvr = new(cnt - dicecash.Count());
                    EffectTrigger(new SimpleSender(currTeam, SenderTag.BeforeRerollDice), dvr);
                    Teams[currTeam].ReRollDice(dvr);
                    break;
                case ActionType.ReRollCard:
                    var cards = Teams[currTeam].CardsInHand;
                    var cardcash0 = cards.Where((value, index) => evt.AdditionalTargetArgs[index] == 0);
                    var cardcash1 = cards.Where((value, index) => evt.AdditionalTargetArgs[index] == 1);
                    cards.Clear();
                    cards.AddRange(cardcash0);
                    var over = Teams[currTeam].LeftCards.Count - cardcash1.Count();
                    Teams[currTeam].RollCard(cardcash1.Count());
                    Teams[currTeam].LeftCards.AddRange(cardcash1);
                    if (over > 0)
                    {
                        Teams[currTeam].RollCard(over);
                    }
                    break;
                case ActionType.Switch:
                case ActionType.SwitchForced:
                    var initial = t.CurrCharacter;
                    t.CurrCharacter = evt.Action.Index;
                    afterEventSender = new SwitchSender(currTeam, initial, t.CurrCharacter);
                    afterEventVariable = new FastActionVariable(evt.Action.Type == ActionType.SwitchForced);
                    break;
                case ActionType.UseSKill:
                    var cha = t.Characters[t.CurrCharacter];
                    var skis = cha.Card.Skills;
                    var ski = skis[evt.Action.Index];
                    //考虑AfterUseAction中可能让角色位置改变的
                    afterEventSender = new UseSkillSender(currTeam, t.CurrCharacter, ski, evt.AdditionalTargetArgs);
                    if (cha.Talent != null && cha.Talent.Card.Skill == evt.Action.Index)
                    {
                        cha.Talent.Card.AfterUseAction(t, t.Characters[t.CurrCharacter], evt.AdditionalTargetArgs);
                    }
                    else
                    {
                        ski.AfterUseAction(t, t.Characters[t.CurrCharacter], evt.AdditionalTargetArgs);
                    }

                    if (ski.Category == SkillCategory.Q)
                    {
                        t.Characters[t.CurrCharacter].MP = 0;
                    }
                    else if (ski.GiveMP)
                    {
                        t.Characters[t.CurrCharacter].MP++;
                    }

                    if (ski is IPersistentProvider<AbstractCardPersistentSummon> su)
                    {
                        //auto summon
                        Teams[currTeam].TryAddSummon(su);
                    }
                    afterEventVariable = new FastActionVariable(false);
                    break;
                case ActionType.UseCard:
                    ActionCard c = t.CardsInHand[evt.Action.Index % t.CardsInHand.Count];
                    t.CardsInHand.Remove(c);
                    c.Card.AfterUseAction(t, evt.AdditionalTargetArgs);
                    if (c.Card is AbstractCardSupport sp)
                    {
                        //auto support
                        Logger.Error($"adding {sp.PersistentPool} for {currTeam}");
                        Teams[currTeam].AddPersistent(sp.PersistentPool, evt.AdditionalTargetArgs?.LastOrDefault() ?? -1);
                    }
                    afterEventVariable = new FastActionVariable(true);
                    break;
                case ActionType.Blend://调和
                    ActionCard c1 = t.CardsInHand[evt.Action.Index % t.CardsInHand.Count];
                    t.CardsInHand.Remove(c1);
                    t.AddDice((int)Teams[currTeam].Characters[Teams[currTeam].CurrCharacter].Card.CharacterElement);
                    afterEventVariable = new FastActionVariable(true);
                    break;
                case ActionType.Pass://空过
                    Logger.Warning($"玩家{currTeam}选择了空过！");
                    t.Pass = true;
                    break;
                default:
                    Logger.Warning($"玩家{currTeam}选择了没有NotImplement的Action！");
                    t.Pass = true;
                    break;
            }
            //after_xx 在这里结算是否是战斗行动
            EffectTrigger(afterEventSender, afterEventVariable);
            bool fight_action = !(afterEventVariable?.Fast ?? false);

            if (fight_action && CurrTeam == currTeam)
            {
                //必须是当前行动的队伍才有意义做出战斗行动
                if (!Teams[1 - CurrTeam].Pass)
                {
                    CurrTeam = 1 - CurrTeam;
                    Logger.Print($"更换了CurrTeam!", ConsoleColor.Cyan);
                }
                else
                {
                    Logger.Print($"{1 - currTeam}已经空过，{currTeam}继续行动！");
                }
            }

        }
    }
}
