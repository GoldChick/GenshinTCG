using TCGAI;
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
    public abstract class AbstractGame
    {
        public AbstractTeam[] Teams { get; protected set; }
        /// <summary>
        /// 对战双方，暂无观战模式
        /// </summary>
        internal AbstractClient[] Clients { get; } = { null, null };


        internal GameStage Stage { get; set; }

        public int Round { get; private set; }

        public AbstractGame()
        {
            Registry.Instance.LoadDlls(Directory.GetCurrentDirectory() + "/mods");
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

                    for (int i = 0; i < 2; i++)
                    {
                        var cardset=Clients[i].RequestCardSet();
                        //TODO:检测是否合理
                        if (true)
                        {

                        }
                    }

                    Logger.Print("开始了一局游戏!");

                    Round++;
                    Logger.Print($"Round {Round}");
                    Logger.Print($"投掷阶段");
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

        protected void InitTeam()
        {

        }
        public void EffectAct()
        {

        }
        public void Step()
        {

        }
        public string GetState()
        {
            return null;
        }
    }
}
