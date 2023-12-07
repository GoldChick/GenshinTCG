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
        /// 处理<b>VALID</b>时间，但是不涉及任何[after_xx状态触发] <br/>
        /// 需要注意的是最好还是触发一下（
        /// </summary>
        internal void EventProcess(NetEvent evt, int currTeam, out AbstractSender afterEventSender, out FastActionVariable? afterEventFastActionVariable)
        {
            var t = Teams[currTeam];

            afterEventSender = new SimpleSender(currTeam, evt.Action.Type.ToSenderTags());
            afterEventFastActionVariable = null;

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
                    break;
                case ActionType.Switch:
                case ActionType.SwitchForced:
                    EffectTrigger(new SimpleSender(currTeam, SenderTag.BeforeSwitch));
                    var initial = t.CurrCharacter;
                    BroadCast(ClientUpdateCreate.CharacterUpdate.SwitchUpdate(currTeam, evt.Action.Index));
                    t.CurrCharacter = evt.Action.Index;
                    afterEventSender = new AfterSwitchSender(currTeam, initial, t.CurrCharacter);
                    afterEventFastActionVariable = new FastActionVariable(evt.Action.Type == ActionType.SwitchForced);
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
                    afterEventFastActionVariable = new FastActionVariable(false);
                    break;
                case ActionType.UseCard:
                    EffectTrigger(new SimpleSender(currTeam, SenderTag.BeforeUseCard));
                    BroadCast(ClientUpdateCreate.CardUpdate(currTeam, ClientUpdateCreate.CardUpdateCategory.Use, evt.Action.Index));

                    var c = t.CardsInHand[evt.Action.Index];

                    if (c.Cost.MPCost > 0)
                    {
                        if (c is IEnergyConsumerCard iec && evt.AdditionalTargetArgs.Length > iec.CostMPFromCharacterIndexInArgs)
                        {
                            t.Characters[t.CurrCharacter].MP -= c.Cost.MPCost;
                        }
                        else
                        {
                            t.Characters[t.CurrCharacter].MP -= c.Cost.MPCost;
                        }
                    }
                    c.AfterUseAction(t, evt.AdditionalTargetArgs);
                    t.CardsInHand.RemoveAt(evt.Action.Index);

                    afterEventSender = new AfterUseCardSender(currTeam, c, evt.AdditionalTargetArgs);
                    afterEventFastActionVariable = new FastActionVariable(true);
                    break;
                case ActionType.Blend://调和
                    t.TryRemoveCard(evt.Action.Index);

                    t.AddSingleDice((int)t.Characters[t.CurrCharacter].Card.CharacterElement);
                    afterEventFastActionVariable = new FastActionVariable(true);
                    break;
                case ActionType.Pass://空过
                    t.Pass = true;
                    break;
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
            Records.Last().Add(record);

            t.SpecialState.HeavyStrike = t.DiceNum % 2 == 0;
            t.SpecialState.DownStrike = evt.Action.Type == ActionType.SwitchForced || evt.Action.Type == ActionType.Switch;
        }
        ///<summary>
        /// 这里处理的事件全都是已经确定valid的
        /// 所以不会进行任何检测
        /// </summary>
        /// <param name="evt">已经证明是valid的NetEvent</param>
        /// <returns>是否是战斗行动</returns>
        internal void HandleEvent(NetEvent evt, int currTeam)
        {
            var t = Teams[currTeam];
            //用于给减费的persistent减少使用次数
            t.GetEventFinalDiceRequirement(evt.Action, true);
            //扣除骰子，所以判断重击应该在这之前的 UseDiceFrom<T>Sender
            t.CostDices(evt.CostArgs);

            //before_xx 通知要发生一个xx事件，[常九爷]等可以开始检测，检测到after_xx之后判定; 发生在process里
            EventProcess(evt, currTeam, out var afterEventSender, out var afterEventFastActionVariable);
            //after_xx 并在这里结算是否是战斗行动
            EffectTrigger(afterEventSender, afterEventFastActionVariable);

            //必须是当前行动的队伍才有意义做出[战斗行动]并且发生[队伍交替]
            if (!(afterEventFastActionVariable?.Fast ?? false) && CurrTeam == currTeam && !Teams[1 - CurrTeam].Pass)
            {
                CurrTeam = 1 - CurrTeam;
            }
        }
        public void TryHandleEvent(NetEvent evt, int teamid)
        {
            if (Teams[teamid].IsEventValid(evt))
            {
                HandleEvent(evt, teamid);
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
