using TCGGame;
using TCGUtil;
using TCGClient;

namespace TCGBase
{
    public abstract class AbstractClient
    {
        public ClientSetting ClientSetting { get; protected set; }
        public ServerSetting ServerSetting { get; protected set; }

        public ReadonlyGame Game { get; protected set; }
        public PlayerTeam Me { get; protected set; }

        /// <summary>
        /// 服务端=>客户端
        /// 客户端链接时更新Setting
        /// </summary>
        public abstract void InitServerSetting(ServerSetting setting);

        /// <summary>
        /// 客户端=>服务端
        /// 游戏开始前传入卡组
        /// </summary>
        public abstract ServerPlayerCardSet RequestCardSet();
        /// <summary>
        /// 客户端=>服务端
        /// 游戏进行中调用索取对应行动
        /// </summary>
        public abstract NetEvent RequestEvent(ActionType demand, string help_txt = "Null");

        /// <summary>
        /// 服务端=>客户端
        /// 游戏进行中更新Team<br/>
        /// </summary>
        public virtual void UpdateTeam(PlayerTeam me) => Me = me;
        /// <summary>
        /// 开发者出于精力问题做出的偷懒的方案，直接传递Full State<br/>
        /// TODO:目前理想的方案其实是每次传递变化量，不过暂时懒得做捏
        /// </summary>
        /// <param name="game"></param>
        public void UpdateGame(ReadonlyGame game) => Game = game;
        public void Update(ClientUpdatePacket packet)
        {

        }
    }
}
