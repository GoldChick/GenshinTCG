using System.Diagnostics;
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
                Logger.Warning("AbstractGame.RequestEvent():无效的NetEvent!");
            }
            Logger.Warning($"AbstractGame.RequestEvent():请求NetEvent超时或者无效，使用默认值！");
            ts.Cancel();
            //TODO:default result
            return new NetEvent(new NetAction(ActionType.Pass));
        }
        public void RequestAndHandleEvent(int teamid, int millisecondsTimeout, ActionType demand, string help_txt = "Null")
        {
            NetEvent ne = RequestEvent(teamid, millisecondsTimeout, demand, help_txt);
            if (HandleEvent(ne, teamid, demand != ActionType.Trival) && CurrTeam == teamid)
            {
                //必须是当前行动的队伍才有意义做出战斗行动
                if (!Teams[1 - CurrTeam].Pass)
                {
                    CurrTeam = 1 - CurrTeam;
                    Logger.Print($"更换了CurrTeam!", ConsoleColor.Cyan);
                }
                else
                {
                    Logger.Print($"{1 - teamid}已经空过，{teamid}继续行动！");
                }
            }
        }
        /// <param name="evt">已经证明是valid的NetEvent</param>
        /// <returns>是否是战斗行动</returns>
        public virtual bool HandleEvent(NetEvent evt, int currTeam, bool forced)
        {
            var t = Teams[currTeam];

            //before_xx 通知要发生一个xx事件，[常九爷]等可以开始检测，检测到after_xx之后判定
            t.EffectTrigger(this, currTeam, new SimpleSender(Tags.SenderTags.ActionTypeToSenderTag(evt.Action.Type, true)));

            PlayerTeam? pt = null;
            AbstractSender afterEventSender = new SimpleSender(Tags.SenderTags.ActionTypeToSenderTag(evt.Action.Type));
            FastActionVariable? afterEventVariable = null;

            if (t is PlayerTeam)
            {
                pt = (PlayerTeam)t;
                //cost
                pt.CostDices(evt.CostArgs);
            }

            switch (evt.Action.Type)
            {
                case ActionType.Switch:
                    var initial = t.CurrCharacter;
                    t.CurrCharacter = evt.Action.Index % t.Characters.Length;
                    //TODO:Check Alive!
                    afterEventSender = new SwitchSender(initial, t.CurrCharacter);
                    if (!forced)
                    {
                        afterEventVariable = new FastActionVariable(false);
                    }
                    break;
                case ActionType.UseSKill:
                    var skis = t.Characters[t.CurrCharacter].Card.Skills;
                    skis[evt.Action.Index % skis.Length].AfterUseAction(this, currTeam);
                    afterEventVariable = new FastActionVariable(false);
                    break;
                case ActionType.UseCard:
                    Debug.Assert(pt != null, "AbstractGame.NetEvent:不应该拥有行动牌的Team尝试打出卡牌！");
                    ActionCard c = pt.CardsInHand[evt.Action.Index % pt.CardsInHand.Count];
                    pt.CardsInHand.Remove(c);
                    c.Card.AfterUseAction(this, currTeam);
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
            t.EffectTrigger(this, currTeam, afterEventSender, afterEventVariable);
            return !(afterEventVariable?.Fast ?? false);
        }
        public virtual bool IsFastAction(NetAction action)
        {
            //蒙德共鸣怎样实现?
            return true;
        }
    }
}
