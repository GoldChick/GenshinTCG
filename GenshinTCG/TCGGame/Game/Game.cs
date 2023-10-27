using System.Text.Json;
using TCGBase;
using TCGClient;
using TCGRule;
using TCGUtil;
namespace TCGGame
{
    internal enum GameStage
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
        internal AbstractClient[] Clients { get; init; }

        internal GameStage Stage { get; set; }

        public int Round { get; private set; }

        public int CurrTeam { get; protected set; }

        public Game()
        {
            Teams = new PlayerTeam[2];

            var c0 = new ConsoleClient();
            var c1 = new SleepClient();

            c0.InitServerSetting(null);

            Clients = new AbstractClient[] { c0, c1 };


            Registry.Instance.LoadDlls(Directory.GetCurrentDirectory() + "/mods");

            //TODO:正式启动不使用
            Registry.Instance.DebugLoad();

            Registry.Instance.Print();
        }
        /// <summary>
        /// 将clients设置完毕之后才能开启
        /// </summary>
        public void StartGame()
        {
            if (Stage == GameStage.PreGame)
            {
                if (Clients.All(p => p != null))
                {
                    Logger.Print("初始化卡组中!");
                    ServerPlayerCardSet[] c = new ServerPlayerCardSet[2];
                    for (int i = 0; i < 2; i++)
                    {
                        c[i] = Clients[i].RequestCardSet();
                        //TODO:检测是否合理
                        //if (true)
                        {
                            //Logger.Print(c[i].CharacterCards.Length);
                            //c[i].Print();
                        }
                    }
                    InitTeam(c[0], c[1]);

                    try
                    {
                        Gaming();
                    }
                    catch (GameOverException)
                    {
                        Logger.Error($"Game Over!");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Game Fatal! ex:{ex.Message}");
                    }
                    finally
                    {
                        Stage = GameStage.PreGame;
                        Logger.Warning($"Final Winner={GetWinner()}");
                    }
                }
                else
                {
                    Logger.Warning("客户端还未完全就位,无法启动!");
                }
            }
            else
            {
                Logger.Warning($"此局游戏已经启动！目前游戏状态：{Stage}");
            }
        }

        public virtual void Gaming()
        {
            for (int i = 0; i < 2; i++)
            {
                Teams[i].RegisterPassive();
                Teams[i].RollCard(5);
            }
            UpdateTeam();
            var t0 = new Task<NetEvent>(() => RequestEvent(0, 30000, ActionType.ReRollCard, "reroll card"));
            var t1 = new Task<NetEvent>(() => RequestEvent(1, 30000, ActionType.ReRollCard, "reroll card"));
            t0.Start();
            t1.Start();
            Task.WaitAll(t0, t1);

            HandleEvent(t0.Result, 0);
            HandleEvent(t1.Result, 1);
            UpdateTeam();

            t0 = new Task<NetEvent>(() => RequestEvent(0, 30000, ActionType.SwitchForced, "Game Start"));
            t1 = new Task<NetEvent>(() => RequestEvent(1, 30000, ActionType.SwitchForced, "Game Start"));
            t0.Start();
            t1.Start();
            Task.WaitAll(t0, t1);

            HandleEvent(t0.Result, 0);
            HandleEvent(t1.Result, 1);
            UpdateTeam();

            EffectTrigger(new SimpleSender(SenderTag.GameStart.ToString()));

            while (!IsGameOver())
            {
                Round++;
                Logger.Print($"Round {Round}");

                Stage = GameStage.Rolling;
                Task.WaitAll(Task.Run(() => Teams[0].RoundStart()), Task.Run(() => Teams[1].RoundStart()));

                Stage = GameStage.Gaming;
                EffectTrigger(new SimpleSender(SenderTag.RoundStart));

                Array.ForEach(Teams, t => t.Pass = false);

                while (!Teams.All(t => t.Pass))
                {
                    //wait until both pass
                    Thread.Sleep(500);
                    var old_currteam = CurrTeam;
                    EffectTrigger(new SimpleSender(CurrTeam, SenderTag.RoundMeStart));

                    UpdateTeam();

                    if (old_currteam == CurrTeam)
                    {
                        Logger.Print($"Team{CurrTeam}正在行动中");
                        RequestAndHandleEvent(CurrTeam, 30000, ActionType.Trival, "Your Turn");
                    }
                    else
                    {
                        throw new Exception("TEAM CURRTEAM 莫名发生了变化");
                    }
                }

                Stage = GameStage.Ending;
                CurrTeam = 1 - CurrTeam;
                EffectTrigger(new SimpleSender(SenderTag.RoundOver.ToString()));

                Array.ForEach(Teams, t => t.RoundEnd());
            }
        }
        protected void InitTeam(ServerPlayerCardSet set0, ServerPlayerCardSet set1)
        {
            //TODO:写的很不完善
            Teams[0] = new PlayerTeam(set0, this, 0);
            Teams[1] = new PlayerTeam(set1, this, 1);

        }
        protected void UpdateTeam()
        {
            for (int i = 0; i < 2; i++)
            {
                Clients[i].UpdateTeam(Teams[i]);
                Clients[i].UpdateGame(new(this, i));
            }
            Logger.Print($"客户端已经更新Team！现在轮到{CurrTeam}行动！", ConsoleColor.DarkBlue);
        }
        public void EffectTrigger(AbstractSender sender, AbstractVariable? variable = null)
        {
            Teams[CurrTeam].EffectTrigger(this, CurrTeam, sender, variable);
            Teams[1 - CurrTeam].EffectTrigger(this, 1 - CurrTeam, sender, variable);
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
