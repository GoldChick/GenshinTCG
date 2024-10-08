﻿namespace TCGBase
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
        internal SimpleSender DefaultSender { get; }
        public PlayerTeam[] Teams { get; init; }
        /// <summary>
        /// 存储非立即结算状态<br/>
        /// 在任意triggerable结算完毕后，会清空queue
        /// </summary>
        internal DelayedTriggerQueue DelayedTriggerQueue { get; }
        /// <summary>
        /// 用来存储[队伍做出的]行动
        /// </summary>
        public List<List<BaseRecord>> NetEventRecords { get; }
        /// <summary>
        /// @desperated<br/>
        /// 用来存储[客观发生的]行动
        /// </summary>
        public List<List<BaseRecord>> ActionRecords { get; }
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
        internal Game(AbstractServer server)
        {
            DefaultSender = new();
            Teams = new PlayerTeam[2];
            Server = server;
            NetEventRecords = new();
            ActionRecords = new();
            DelayedTriggerQueue = new();
            GlobalPersistents = new();
        }
        internal void StartGame(IEnumerable<ServerPlayerCardSet> cardsets, IEnumerable<AbstractClient> playerClients)
        {
            if (Stage == GameStage.PreGame)
            {
                Teams[0] = new(cardsets.ElementAt(0), this, 0);
                Teams[1] = new(cardsets.ElementAt(1), this, 1);
                playerClients.ElementAt(0).BindTeam(Teams[0]);
                playerClients.ElementAt(1).BindTeam(Teams[1]);
                Stage = GameStage.PreGame;
                Gaming();
            }
            else
            {
                throw new Exception($"此局游戏已经启动！目前游戏状态：{Stage}");
            }
        }
        protected virtual void Gaming()
        {
            _currteam = 0;

            NetEventRecords.Add(new());

            Teams[0].RollCard(5);
            Teams[1].RollCard(5);

            Task.WaitAll(Task.Run(() => RequestEvent(0, 30000, OperationType.ReRollCard)), Task.Run(() => RequestEvent(1, 30000, OperationType.ReRollCard)));

            var t0 = new Task<NetEvent>(() => RequestEvent(CurrTeam, 30000, OperationType.Switch));
            var t1 = new Task<NetEvent>(() => RequestEvent(1 - CurrTeam, 30000, OperationType.Switch));
            t0.Start();
            t1.Start();
            Task.WaitAll(t0, t1);

            HandleNetEvent(t0.Result, CurrTeam, OperationType.Switch);
            HandleNetEvent(t1.Result, 1 - CurrTeam, OperationType.Switch);

            DelayedTriggerQueue.Active = true;
            foreach (var team in Teams)
            {
                foreach (var c in team.Characters)
                {
                    EffectTrigger(SenderTag.OnCharacterOn, new OnCharacterOnSender(team.TeamID, c, true));
                }
            }

            while (!IsGameOver())
            {
                Round++;
                NetEventRecords.Add(new());

                DelayedTriggerQueue.TryEmpty();
                Stage = GameStage.Rolling;
                Task.WaitAll(Task.Run(() => Teams[CurrTeam].RoundStart()), Task.Run(() => Teams[1 - CurrTeam].RoundStart()));

                Stage = GameStage.Gaming;
                EffectTrigger(SenderTag.RoundStart, DefaultSender);

                Array.ForEach(Teams, t => t.Pass = false);

                while (!Teams.All(t => t.Pass))
                {
                    //wait until both pass
                    var oldteam = CurrTeam;
                    EffectTrigger(SenderTag.RoundMeStart, new SimpleSender(CurrTeam));
                    EffectTrigger(SenderTag.RoundDuring, new SimpleSender(CurrTeam));
                    if (oldteam == CurrTeam)
                    {
                        RequestAndHandleEvent(CurrTeam, 30000, OperationType.Trivial);
                    }
                }

                Stage = GameStage.Ending;
                EffectTrigger(SenderTag.RoundOver, DefaultSender);
                Teams[CurrTeam].RoundEnd();
                Teams[1 - CurrTeam].RoundEnd();
                EffectTrigger(SenderTag.RoundStep, DefaultSender);
            }
        }
        internal void EffectTrigger(SenderTag sendertag, SimpleSender sender, AbstractVariable? variable = null, bool broadcast = true)
            => EffectTrigger(sendertag.ToString(), sender, variable, broadcast);

        internal void EffectTrigger(string sendertag, SimpleSender sender, AbstractVariable? variable = null, bool broadcast = true)
                => DelayedTriggerQueue.TryTrigger(() => InstantTrigger(sendertag, sender, variable, broadcast, false));
        /// <summary>
        /// 按照 当前队伍-另一队伍；出战角色-出战-其他角色-召唤-支援 顺序，依次<b>立即</b>触发双方所有状态<br/>
        /// 这里的状态都不会改变CurrTeam
        /// </summary>
        internal void InstantTrigger(string sendertag, SimpleSender sender, AbstractVariable? variable = null, bool broadcast = true, bool instant = true)
        {
            List<EventPersistentSetHandler> handlers = new();

            handlers.AddRange(Teams[CurrTeam].GetEffectHandlers(sendertag, sender));
            handlers.AddRange(Teams[1 - CurrTeam].GetEffectHandlers(sendertag, sender));

            if (instant)
            {
                foreach (var h in handlers)
                {
                    h.Invoke(sender, variable);
                }
            }
            else
            {
                foreach (var h in handlers)
                {
                    DelayedTriggerQueue.Active = true;
                    h.Invoke(sender, variable);
                    DelayedTriggerQueue.TryEmpty();
                }
            }

            if (broadcast)
            {
                BroadCastRegion();
            }
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
