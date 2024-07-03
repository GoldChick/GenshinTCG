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
            NetEventRecords.Add(new());
            for (int i = 0; i < 2; i++)
            {
                Teams[i].RollCard(5);
            }
            var t0 = new Task<NetEvent>(() => RequestEvent(0, 30000, OperationType.ReRollCard));
            var t1 = new Task<NetEvent>(() => RequestEvent(1, 30000, OperationType.ReRollCard));
            t0.Start();
            t1.Start();
            Task.WaitAll(t0, t1);

            HandleNetEvent(t0.Result, 0, OperationType.ReRollCard);
            HandleNetEvent(t1.Result, 1, OperationType.ReRollCard);

            t0 = new Task<NetEvent>(() => RequestEvent(0, 30000, OperationType.Switch));
            t1 = new Task<NetEvent>(() => RequestEvent(1, 30000, OperationType.Switch));
            t0.Start();
            t1.Start();
            Task.WaitAll(t0, t1);

            HandleNetEvent(t0.Result, 0, OperationType.Switch);
            HandleNetEvent(t1.Result, 1, OperationType.Switch);

            DelayedTriggerQueue.Active = true;
            foreach (var team in Teams)
            {
                foreach (var c in team.Characters)
                {
                    EffectTrigger(new OnCharacterOnSender(team.TeamIndex, c, true));
                }
            }

            while (!IsGameOver())
            {
                Round++;
                NetEventRecords.Add(new());

                Stage = GameStage.Rolling;
                Task.WaitAll(Task.Run(() => Teams[0].RoundStart()), Task.Run(() => Teams[1].RoundStart()));

                Stage = GameStage.Gaming;
                DelayedTriggerQueue.TryEmpty();
                EffectTrigger(new SimpleSender(SenderTag.RoundStart));

                Array.ForEach(Teams, t => t.Pass = false);

                while (!Teams.All(t => t.Pass))
                {
                    //wait until both pass
                    var oldteam = CurrTeam;
                    EffectTrigger(new SimpleSender(CurrTeam, SenderTag.RoundMeStart));
                    EffectTrigger(new SimpleSender(CurrTeam, SenderTag.RoundDuring));
                    if (oldteam == CurrTeam)
                    {
                        RequestAndHandleEvent(CurrTeam, 30000, OperationType.Trival);
                    }
                }

                Stage = GameStage.Ending;
                EffectTrigger(new SimpleSender(SenderTag.RoundOver));
                Teams[CurrTeam].RoundEnd();
                Teams[1 - CurrTeam].RoundEnd();
                EffectTrigger(new SimpleSender(SenderTag.RoundStep));
            }
        }
        public void EffectTrigger(AbstractSender sender, AbstractVariable? variable = null, bool broadcast = true)
                => DelayedTriggerQueue.TryTrigger(() => InstantTrigger(sender, variable, broadcast, false));
        /// <summary>
        /// 按照 当前队伍-另一队伍；出战角色-出战-其他角色-召唤-支援 顺序，依次<b>立即</b>触发双方所有状态<br/>
        /// 这里的状态都不会改变CurrTeam
        /// </summary>
        internal void InstantTrigger(AbstractSender sender, AbstractVariable? variable = null, bool broadcast = true, bool instant = true)
        {
            List<EventPersistentSetHandler> handlers = new();

            handlers.AddRange(Teams[CurrTeam].GetEffectHandlers(sender));
            handlers.AddRange(Teams[1 - CurrTeam].GetEffectHandlers(sender));

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
