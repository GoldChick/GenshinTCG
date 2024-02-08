namespace TCGBase
{
    public partial class Game
    {
        internal void RequestAndHandleEvent(int teamid, int millisecondsTimeout, OperationType demand) => HandleNetEvent(RequestEvent(teamid, millisecondsTimeout, demand), teamid, demand);
        /// <summary>
        /// 向对应客户端请求一个valid的事件，或者default的事件
        /// </summary>
        /// <returns>返回的必然是有效事件</returns>
        private NetEvent RequestEvent(int teamid, int millisecondsTimeout, OperationType demand)
        {
            CancellationTokenSource ts = new();
            var t = new Task<NetEvent>(() => Clients[teamid].RequestEvent(demand), ts.Token);
            Clients[1 - teamid].RequestEnemyEvent(demand);
            t.Start();

            Task.WaitAny(t, Task.Run(() => Thread.Sleep(millisecondsTimeout)));

            if (t.IsCompleted && Teams[teamid].IsEventValid(t.Result) && (demand == OperationType.Trival || demand == t.Result.Operation.Type))
            {
                return t.Result;
            }
            ts.Cancel();

            NetEvent? evt = null;
            switch (demand)
            {
                case OperationType.ReRollCard:
                    evt = new NetEvent(new NetOperation(demand), new int[8], Enumerable.Repeat(0, Teams[teamid].CardsInHand.Count()).ToArray());
                    break;
                case OperationType.ReRollDice:
                    evt = new NetEvent(new NetOperation(demand), new int[8], Enumerable.Repeat(0, Teams[teamid].Dices.Count).ToArray());
                    break;
                case OperationType.Switch:
                    {
                        var chas = Teams[teamid].Characters;
                        for (int i = 0; i < chas.Length; i++)
                        {
                            if (chas[i].Alive)
                            {
                                evt = new NetEvent(new NetOperation(demand, i));
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
                    evt = new NetEvent(new NetOperation(OperationType.Pass));
                    break;
            }
            return evt;
        }
        ///<summary>
        /// 这里处理的事件全都是已经确定valid的，所以不会进行任何检测<br/>
        /// <b>注：这里处理的是事件，不完全等于行动</b>
        /// </summary>
        /// <param name="evt">已经证明是valid的NetEvent</param>
        internal void HandleNetEvent(NetEvent evt, int teamid, OperationType demand = OperationType.Trival)
        {
            var team = Teams[teamid];

            FastActionVariable afterEventFastActionVariable = new(false);

            if (demand == OperationType.Trival)
            {
                /*
                 * 行动一览：
                 * 切换角色+使用技能+使用卡牌+调和卡牌+暂时空过+回合空过
                 */
                //用于给减费的persistent减少使用次数
                team.GetEventFinalDiceRequirement(evt.Operation, true);
                team.CostDices(evt.CostArgs);
                //TODO:消耗充能在哪里
            }
            switch (evt.Operation.Type)
            {
                case OperationType.ReRollDice:
                    var dices = team.Dices;
                    int cnt = dices.Count;
                    var dicecash = dices.Where((value, index) => evt.AdditionalTargetArgs[index] == 0).ToList();
                    dices.Clear();
                    dices.AddRange(dicecash);
                    DiceRollingVariable dvr = new(cnt - dicecash.Count);
                    EffectTrigger(new SimpleSender(teamid, SenderTag.BeforeRerollDice), dvr);
                    team.ReRollDice(dvr);
                    break;
                case OperationType.ReRollCard:
                    //TODO:换牌
                    //var cards = t.CardsInHand;
                    //var cardcash0 = cards.Where((value, index) => evt.AdditionalTargetArgs[index] == 0).ToList();
                    //var cardcash1 = cards.Where((value, index) => evt.AdditionalTargetArgs[index] == 1).ToList();
                    //BroadCast(ClientUpdateCreate.CardUpdate(currTeam, ClientUpdateCreate.CardUpdateCategory.Push, cards.Select((value, index) => evt.AdditionalTargetArgs[index] == 1 ? index : -1).Where(p => p >= 0).ToArray()));
                    //cards.Clear();
                    //cards.AddRange(cardcash0);
                    //var over = cardcash1.Count - t.LeftCards.Count;
                    //t.RollCard(cardcash1.Count);
                    //t.LeftCards.AddRange(cardcash1);
                    //if (over > 0)
                    //{
                    //    t.RollCard(over);
                    //}
                    //TODO:优先不换上来换下去的种类
                    break;
                case OperationType.Switch:
                    team.TrySwitchToIndex(evt.Operation.Index);
                    break;
                case OperationType.UseSKill:
                    EffectTrigger(new ActionUseSkillSender(team.TeamIndex, team.CurrCharacter, evt.Operation.Index));
                    break;
                case OperationType.UseCard:
                    afterEventFastActionVariable.Fast = (team.CardsInHand[evt.Operation.Index].CardBase as AbstractCardAction)?.FastAction ?? true;
                    BroadCast(ClientUpdateCreate.CardUpdate(teamid, ClientUpdateCreate.CardUpdateCategory.Use, evt.Operation.Index));
                    EffectTrigger(new ActionUseCardSender(team.TeamIndex, evt.Operation.Index, evt.AdditionalTargetArgs));
                    break;
                case OperationType.Blend://调和
                    team.CardsInHand.TryDestroyAt(evt.Operation.Index);
                    team.AddSingleDice((int)team.Characters[team.CurrCharacter].CharacterCard.CharacterElement);
                    afterEventFastActionVariable.Fast = true;
                    break;
                case OperationType.Break://行动空过
                    break;
                default://回合空过
                    team.Pass = true;
                    break;
            }

            if (demand == OperationType.Trival)
            {
                EffectTrigger(new AfterOperationSender(teamid, evt.Operation.Type), afterEventFastActionVariable);
                //必须是当前行动的队伍才有意义做出[战斗行动]并且发生[队伍交替]
                if (CurrTeam == teamid)
                {
                    if (team.Pass || (!afterEventFastActionVariable.Fast && !team.Enemy.Pass))
                    {
                        CurrTeam = 1 - CurrTeam;
                    }
                }
            }
            //update team state
            team.SpecialState.HeavyStrike = team.DiceNum % 2 == 0;
        }
        public void TryHandleEvent(NetEvent evt, int teamid)
        {
            if (Teams[teamid].IsEventValid(evt))
            {
                HandleNetEvent(evt, teamid);
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
