﻿using TCGGame;
using TCGUtil;
using TCGBase;
namespace TCGClient
{
    internal abstract class AbstractClient
    {
        public ClientSetting ClientSetting { get; protected set; }
        public ServerSetting ServerSetting { get; protected set; }

        public PlayerTeam Me { get; protected set; }
        public PlayerTeam Enemy { get; protected set; }


        /// <summary>
        /// 服务端=>客户端
        /// 客户端链接时更新Setting
        /// </summary>
        public abstract void InitServerSetting(ServerSetting setting);


        /// <summary>
        /// 客户端=>服务端
        /// 游戏开始前传入卡组
        /// </summary>
        public abstract AbstractServerCardSet RequestCardSet();
        /// <summary>
        /// 客户端=>服务端
        /// 游戏进行中传入行动
        /// </summary>
        public abstract NetEvent RequestEvent(ActionType demand, string help_txt = "Null");

        /// <summary>
        /// 服务端=>客户端
        /// 游戏进行中更新Team
        /// </summary>
        public virtual void UpdateTeam(PlayerTeam me, PlayerTeam enemy)
        {
            Me = me;
            Enemy = enemy;
        }
        public void Update(ClientUpdatePacket packet)
        {

        }
    }
}
