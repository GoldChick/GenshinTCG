namespace TCGBase
{
    public partial class Game
    {
        public void RequestAndHandleEvent(int teamid, int millisecondsTimeout, ActionType demand) => HandleEvent(RequestEvent(teamid, millisecondsTimeout, demand), teamid);
        /// <summary>
        /// 向对应客户端请求一个valid的事件，或者default的事件
        /// </summary>
        /// <returns>返回的必然是有效事件</returns>
        private NetEvent RequestEvent(int teamid, int millisecondsTimeout, ActionType demand)
        {
            CancellationTokenSource ts = new();
            var t = new Task<NetEvent>(() => Clients[teamid].RequestEvent(demand), ts.Token);
            Clients[1 - teamid].RequestEnemyEvent(demand);
            t.Start();

            Task.WaitAny(t, Task.Run(() => Thread.Sleep(millisecondsTimeout)));

            if (t.IsCompleted && Teams[teamid].IsEventValid(t.Result) && (demand == ActionType.Trival || demand == t.Result.Action.Type))
            {
                return t.Result;
            }
            ts.Cancel();

            NetEvent? evt = null;
            switch (demand)
            {
                case ActionType.ReRollCard:
                    evt = new NetEvent(new NetAction(demand), new int[8], Enumerable.Repeat(0, Teams[teamid].CardsInHand.Count).ToArray());
                    break;
                case ActionType.ReRollDice:
                    evt = new NetEvent(new NetAction(demand), new int[8], Enumerable.Repeat(0, Teams[teamid].Dices.Count).ToArray());
                    break;
                case ActionType.SwitchForced:
                    {
                        var chas = Teams[teamid].Characters;
                        for (int i = 0; i < chas.Length; i++)
                        {
                            if (chas[i].Alive)
                            {
                                evt = new NetEvent(new NetAction(demand, i));
                                break;
                            }
                        }
                        if (evt == null)
                        {
                            throw new Exception("已经没有存活角色，仍然要求SwitchForced!");
                        }
                    }
                    break;
                default:
                    evt = new NetEvent(new NetAction(ActionType.Pass));
                    break;
            }
            return evt;
        }
        ///<summary>
        /// 处理<b>VALID</b>事件，可能为[行动]<br/>
        /// </summary>
        /// <returns>是否是快速行动（对于[非行动]默认为true,可以舍弃）</returns>
        private bool EventMayActionProcess(NetEvent evt, int currTeam, out AbstractSender afterEventSender, bool isaction = false)
        {
            var t = Teams[currTeam];

            afterEventSender = new SimpleSender(currTeam, evt.Action.Type.ToSenderTags());
            FastActionVariable? afterEventFastActionVariable = isaction ? new FastActionVariable(false) : null;
            switch (evt.Action.Type)
            {
                case ActionType.Switch:
                    EffectTrigger(new SimpleSender(currTeam, SenderTag.BeforeSwitch));
                    var initial = t.CurrCharacter;
                    t.CurrCharacter = evt.Action.Index;
                    afterEventSender = new AfterSwitchSender(currTeam, initial, t.CurrCharacter);
                    break;
                case ActionType.UseSKill:
                    EffectTrigger(new SimpleSender(currTeam, SenderTag.BeforeUseSkill));
                    var cha = t.Characters[t.CurrCharacter];
                    var ski = cha.Card.Skills[evt.Action.Index];
                    t.AddPersistent(new Effect_RoundSkillCounter(ski), t.CurrCharacter);
                    //考虑AfterUseAction中可能让角色位置改变的
                    afterEventSender = new AfterUseSkillSender(currTeam, cha, ski, evt.AdditionalTargetArgs);

                    if (ski.Cost.MPCost > 0)
                    {
                        t.Characters[t.CurrCharacter].MP -= ski.Cost.MPCost;
                    }
                    else if (ski.GiveMP)
                    {
                        t.Characters[t.CurrCharacter].MP++;
                    }

                    if (cha.Effects.Find(-3)?.Card is AbstractCardEquipmentOverrideSkillTalent pt && pt.Skill == evt.Action.Index)
                    {
                        pt.TalentTriggerAction(t, t.Characters[t.CurrCharacter], evt.AdditionalTargetArgs);
                    }
                    else
                    {
                        ski.AfterUseAction(t, t.Characters[t.CurrCharacter], evt.AdditionalTargetArgs);
                    }
                    break;
                case ActionType.UseCard:
                    EffectTrigger(new SimpleSender(currTeam, SenderTag.BeforeUseCard));
                    BroadCast(ClientUpdateCreate.CardUpdate(currTeam, ClientUpdateCreate.CardUpdateCategory.Use, evt.Action.Index));

                    AbstractCardAction c = t.CardsInHand[evt.Action.Index];

                    if (c.Cost.MPCost > 0)
                    {
                        if (c is IEnergyConsumerCard iec && evt.AdditionalTargetArgs.Length > iec.CostMPFromCharacterIndexInArgs)
                        {
                            t.Characters[evt.AdditionalTargetArgs[iec.CostMPFromCharacterIndexInArgs]].MP -= c.Cost.MPCost;
                        }
                        else
                        {
                            t.Characters[t.CurrCharacter].MP -= c.Cost.MPCost;
                        }
                    }
                    c.AfterUseAction(t, evt.AdditionalTargetArgs);
                    t.CardsInHand.RemoveAt(evt.Action.Index);

                    afterEventSender = new AfterUseCardSender(currTeam, c, evt.AdditionalTargetArgs);
                    afterEventFastActionVariable = new FastActionVariable(c.FastAction);
                    break;
                case ActionType.Blend://调和
                    t.TryRemoveCard(evt.Action.Index);
                    t.AddSingleDice((int)t.Characters[t.CurrCharacter].Card.CharacterElement);
                    afterEventFastActionVariable = new FastActionVariable(true);
                    break;
                case ActionType.Break://行动空过
                    break;
                default://回合空过
                    t.Pass = true;
                    break;
            }
            EffectTrigger(afterEventSender, afterEventFastActionVariable);
            return afterEventFastActionVariable?.Fast ?? true;
        }
        /// <summary>
        /// 重投+换牌+强制切人
        /// </summary>
        private void EventNotActionProcess(NetEvent evt, int currTeam)
        {
            var t = Teams[currTeam];
            switch (evt.Action.Type)
            {
                case ActionType.ReRollDice:
                    var dices = t.Dices;
                    int cnt = dices.Count;
                    var dicecash = dices.Where((value, index) => evt.AdditionalTargetArgs[index] == 0).ToList();
                    dices.Clear();
                    dices.AddRange(dicecash);
                    DiceRollingVariable dvr = new(cnt - dicecash.Count);
                    EffectTrigger(new SimpleSender(currTeam, SenderTag.BeforeRerollDice), dvr);
                    t.ReRollDice(dvr);
                    break;
                case ActionType.ReRollCard:
                    var cards = t.CardsInHand;
                    var cardcash0 = cards.Where((value, index) => evt.AdditionalTargetArgs[index] == 0).ToList();
                    var cardcash1 = cards.Where((value, index) => evt.AdditionalTargetArgs[index] == 1).ToList();
                    BroadCast(ClientUpdateCreate.CardUpdate(currTeam, ClientUpdateCreate.CardUpdateCategory.Push, cards.Select((value, index) => evt.AdditionalTargetArgs[index] == 1 ? index : -1).Where(p => p >= 0).ToArray()));
                    cards.Clear();
                    cards.AddRange(cardcash0);
                    var over = cardcash1.Count - t.LeftCards.Count;
                    t.RollCard(cardcash1.Count);
                    t.LeftCards.AddRange(cardcash1);
                    if (over > 0)
                    {
                        t.RollCard(over);
                    }
                    //TODO:优先不换上来换下去的种类
                    break;
                case ActionType.SwitchForced:
                    var initial = t.CurrCharacter;
                    t.CurrCharacter = evt.Action.Index;
                    EffectTrigger(new AfterSwitchSender(currTeam, initial, t.CurrCharacter), null);
                    break;
            }
        }
        ///<summary>
        /// 这里处理的事件全都是已经确定valid的，所以不会进行任何检测<br/>
        /// <b>注：这里处理的是事件，不等于行动</b>
        /// </summary>
        /// <param name="evt">已经证明是valid的NetEvent</param>
        /// <returns>是否是战斗行动</returns>
        internal void HandleEvent(NetEvent evt, int currTeam)
        {
            AbstractSender? afterEventSender = null;
            var t = Teams[currTeam];
            /*
             * 行动一览：
             * 切换角色+使用技能+使用卡牌+调和卡牌+暂时空过+回合空过
             */
            if ((int)evt.Action.Type >= 4)
            {
                //用于给减费的persistent减少使用次数
                t.GetEventFinalDiceRequirement(evt.Action, true);
                t.CostDices(evt.CostArgs);

                //TODO:触发[任意行动]后

                //必须是当前行动的队伍才有意义做出[战斗行动]并且发生[队伍交替]
                if (!EventMayActionProcess(evt, currTeam, out afterEventSender, true) && CurrTeam == currTeam && !Teams[1 - CurrTeam].Pass)
                {
                    CurrTeam = 1 - CurrTeam;
                }
            }
            else
            {
                EventNotActionProcess(evt, currTeam);
            }

            NetEventRecord record;
            if (afterEventSender is AfterUseSkillSender ss)
            {
                record = new UseSkillRecord(currTeam, evt, ss.Character, ss.Skill);
            }
            else if (afterEventSender is AfterUseCardSender cs)
            {
                record = new UseCardRecord(currTeam, evt, cs.Card);
            }
            else
            {
                record = new(currTeam, evt);
            }
            NetEventRecords.Last().Add(record);
            //update team state
            t.SpecialState.HeavyStrike = t.DiceNum % 2 == 0;
            t.SpecialState.DownStrike = evt.Action.Type == ActionType.SwitchForced || evt.Action.Type == ActionType.Switch;
        }
        public void TryHandleEvent(NetEvent evt, int teamid)
        {
            if (Teams[teamid].IsEventValid(evt))
            {
                HandleEvent(evt, teamid);
            }
        }
        /// <summary>
        /// 这里处理所有被动的行为，并且默认行为可行（不是行动！）
        /// </summary>
        public void TryProcessEvent(NetEvent evt, int teamid)
        {
            if ((int)evt.Action.Type >= 4)
            {
                EventMayActionProcess(evt, teamid, out _);
            }
            else
            {
                EventNotActionProcess(evt, teamid);
            }
        }

        /// <summary>
        /// teamme: 发起换边请求的队伍<br/>
        /// 当这个队伍是当前队伍时，才能有效切换
        /// </summary>
        internal void TrySwitchSide(int teamme)
        {
            if (CurrTeam == teamme)
            {
                CurrTeam = 1 - teamme;
            }
        }
    }
}
