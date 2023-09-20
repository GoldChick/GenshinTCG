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
    public abstract partial class AbstractGame
    {
        public AbstractTeam[] Teams { get; init; }
        /// <summary>
        /// 对战双方，暂无观战模式
        /// </summary>
        internal AbstractClient[] Clients { get; init; }

        internal GameStage Stage { get; set; }

        public int Round { get; private set; }

        //TODO:蒙德共鸣
        public int CurrTeam { get; protected set; }

        public AbstractGame()
        {
            Teams = new AbstractTeam[2];
            Clients = new AbstractClient[2];

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
                        c[i] = Clients[i].RequestCardSet() as ServerPlayerCardSet;
                        //TODO:检测是否合理
                        if (true)
                        {
                            Logger.Print(c[i].CharacterCards.Length);
                            c[i].Print();
                        }
                    }
                    InitTeam(c[0], c[1]);

                    Logger.Print("开始了一局游戏!");

                    Gaming();

                    Logger.Warning($"Final Winner={GetWinner()}");
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
            UpdateTeam();
            var t0 = Clients[0].RequestEvent(ActionType.Switch, "Game Start");
            var t1 = Clients[1].RequestEvent(ActionType.Switch, "Game Start");
            t0.Start();
            t1.Start();
            Task.WaitAll(t0, t1);

            HandleEvent(t0.Result, 0);
            HandleEvent(t1.Result, 1);
            
            UpdateTeam();

            //TODO:出角色
            while (!IsGameOver())
            {
                Round++;
                Logger.Print($"Round {Round}");
                CurrTeam = 1 - CurrTeam;

                Array.ForEach(Teams, t => t.RoundStart());

                //wait until both ready

                Stage = GameStage.Rolling;
                Logger.Print($"投掷阶段");

                Thread.Sleep(500);
                Stage = GameStage.Gaming;
                EffectTrigger(new SimpleSender(Tags.SenderTags.ROUND_START));
                Logger.Print($"行动阶段");

                Array.ForEach(Teams, t => t.Pass = false);
                //TODO:先后手兑换

                while (!Teams.All(t => t.Pass))
                {
                    //wait until both pass
                    Thread.Sleep(500);
                    Logger.Print($"{CurrTeam}正在行动中");

                    UpdateTeam();
                    //TODO:Request计时!!!!
                    var t = Clients[CurrTeam].RequestEvent(ActionType.Trival, "Your Turn");
                    t.Start();
                    var evt = t.Result;

                    Logger.Warning($"Event Requirement:{JsonSerializer.Serialize(Teams[CurrTeam].GetEventRequirement(evt.Action))}");
                    bool valid = Teams[CurrTeam].IsEventValid(evt);
                    Logger.Warning($"Event Valid:{valid}");

                    if (valid && HandleEvent(evt, CurrTeam))
                    {
                        CurrTeam = 1 - CurrTeam;
                    }
                }

                Stage = GameStage.Ending;
                Logger.Print($"结束阶段");
                EffectTrigger(new SimpleSender(Tags.SenderTags.ROUND_OVER));

                Array.ForEach(Teams, t => t.RoundEnd());
            }
        }
        protected void InitTeam(AbstractServerCardSet set0, AbstractServerCardSet set1)
        {
            //TODO:写的很不完善
            Logger.Warning("initing teams");
            Teams[0] = new PlayerTeam(set0 as ServerPlayerCardSet, this, 0);
            Teams[1] = new PlayerTeam(set1 as ServerPlayerCardSet, this, 1);

        }
        protected void UpdateTeam()
        {
            for (int i = 0; i < 2; i++)
            {
                Clients[i].UpdateTeam(Teams[i], Teams[1 - i]);
            }
            Logger.Print($"客户端已经更新Team！现在轮到{CurrTeam}行动！", ConsoleColor.DarkBlue);

        }
        public void EffectTrigger(AbstractSender sender, AbstractVariable? variable = null)
        {
            Logger.Warning($"Effect Triggering : sender {sender.SenderName}");

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
        public void Step()
        {
            Logger.Print("step once");
        }
        public string GetState()
        {
            return null;
        }
    }
}
