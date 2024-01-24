﻿using Minecraft;

namespace TCGBase
{
    public enum GameStage
    {
        PreGame,

        Rolling,
        Gaming,
        Ending,
    }
    public partial class Game
    {
        public PlayerTeam[] Teams { get; init; }
        /// <summary>
        /// 对战双方，暂无观战模式
        /// </summary>
        internal List<AbstractClient> Clients { get; init; }

        public GameStage Stage { get; private set; }

        public int Round { get; private set; }
        private int _currteam;
        public int CurrTeam
        {
            get => _currteam; protected set
            {
                _currteam = value;
                BroadCast(ClientUpdateCreate.CurrTeamUpdate(value));
            }
        }
        internal bool InstantTrigger { get; set; }
        internal Stack<Action> DelayedTriggerStack { get; set; }
        public Game()
        {
            Teams = new PlayerTeam[2];
            Clients = new();
            NetEventRecords = new();
        }
        public void AddClient(AbstractClient c) => Clients.Add(c);
        /// <summary>
        /// 将clients设置完毕之后才能开启
        /// </summary>
        public void StartGame()
        {
            if (Stage == GameStage.PreGame)
            {
                if (Clients.Count >= 2)
                {
                    ServerPlayerCardSet[] c = new ServerPlayerCardSet[2];
                    for (int i = 0; i < 2; i++)
                    {
                        try
                        {
                            c[i] = Clients[i].RequestCardSet();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"编号为{i}的玩家的卡组无效！报错原因{ex.Message}");
                        }
                        if (!c[i].Valid)
                        {
                            throw new Exception($"编号为{i}的玩家的卡组无效！可能的原因：检测到无效卡、数量不为3和30、携带了无法携带的卡。");
                        }
                    }
                    InitTeam(c[0], c[1]);

                    Gaming();
                    Stage = GameStage.PreGame;
                }
                else
                {
                    throw new Exception("客户端还未完全就位,无法启动!");
                }
            }
            else
            {
                throw new Exception($"此局游戏已经启动！目前游戏状态：{Stage}");
            }
        }

        public virtual void Gaming()
        {
            NetEventRecords.Add(new());
            for (int i = 0; i < 2; i++)
            {
                Teams[i].RegisterPassive();
                Teams[i].RollCard(5);
            }
            var t0 = new Task<NetEvent>(() => RequestEvent(0, 30000, ActionType.ReRollCard));
            var t1 = new Task<NetEvent>(() => RequestEvent(1, 30000, ActionType.ReRollCard));
            t0.Start();
            t1.Start();
            Task.WaitAll(t0, t1);

            HandleEvent(t0.Result, 0);
            HandleEvent(t1.Result, 1);

            t0 = new Task<NetEvent>(() => RequestEvent(0, 30000, ActionType.SwitchForced));
            t1 = new Task<NetEvent>(() => RequestEvent(1, 30000, ActionType.SwitchForced));
            t0.Start();
            t1.Start();
            Task.WaitAll(t0, t1);

            HandleEvent(t0.Result, 0);
            HandleEvent(t1.Result, 1);

            EffectTrigger(new SimpleSender(SenderTag.GameStart));

            while (!IsGameOver())
            {
                Round++;
                NetEventRecords.Add(new());

                Stage = GameStage.Rolling;
                Task.WaitAll(Task.Run(() => Teams[0].RoundStart()), Task.Run(() => Teams[1].RoundStart()));

                Stage = GameStage.Gaming;
                EffectTrigger(new SimpleSender(SenderTag.RoundStart));

                Array.ForEach(Teams, t => t.Pass = false);

                while (!Teams.All(t => t.Pass))
                {
                    //wait until both pass
                    var oldteam = CurrTeam;
                    EffectTrigger(new SimpleSender(CurrTeam, SenderTag.RoundMeStart));
                    if (oldteam == CurrTeam)
                    {
                        RequestAndHandleEvent(CurrTeam, 30000, ActionType.Trival);
                    }
                }

                Stage = GameStage.Ending;
                CurrTeam = 1 - CurrTeam;
                EffectTrigger(new SimpleSender(SenderTag.RoundOver));
                Teams[CurrTeam].RoundEnd();
                Teams[1 - CurrTeam].RoundEnd();
                EffectTrigger(new SimpleSender(SenderTag.RoundStep));
            }
        }
        protected void InitTeam(ServerPlayerCardSet set0, ServerPlayerCardSet set1)
        {
            //TODO:写的很不完善
            Teams[0] = new PlayerTeam(set0, this, 0);
            Teams[1] = new PlayerTeam(set1, this, 1);
            Clients[0].BindTeam(Teams[0]);
            Clients[1].BindTeam(Teams[1]);
        }
        /// <summary>
        /// 如果你确信此次effecttrigger不会改变variable，那么可以不检测teamid
        /// </summary>
        public void EffectTrigger(AbstractSender sender, AbstractVariable? variable = null, bool broadcast = true)
        {
            Teams[CurrTeam].EffectTrigger(sender, variable);
            Teams[1 - CurrTeam].EffectTrigger(sender, variable);
            if (broadcast)
            {
                BroadCastRegion();
            }
        }
        public void EffectUpdate()
        {
            Teams[CurrTeam].EffectUpdate();
            Teams[1 - CurrTeam].EffectUpdate();
        }
        public bool IsGameOver() => Round > 15 || Teams.Any(t => t.Characters.All(c => !c.Alive));
        public int GetWinner()
        {
            for (int i = 0; i < 2; i++)
                if (Teams[1 - i].Characters.All(c => !c.Alive))
                    return i;
            return -1;
        }
    }
}
