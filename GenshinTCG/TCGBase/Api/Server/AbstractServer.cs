using System.Diagnostics;

namespace TCGBase
{
    public enum ServerState
    {
        WaitingForBoth,
        WaitingForOne,
        WaitingForStart,
        Gaming,
    }
    /// <summary>
    /// 所有有效信息都保存在Server中<br/>
    /// Client只给出预览效果
    /// </summary>
    public abstract class AbstractServer
    {
        public ServerConfig Config { get; protected set; }
        public Game Game { get; }
        public ServerState State { get; protected set; }
        public List<AbstractClient> PlayerClients { get; }
        public List<AbstractClient> OtherClients { get; }
        protected AbstractServer()
        {
            Config = new();
            Game = new(this);
            PlayerClients = new();
            OtherClients = new();
            State = ServerState.WaitingForBoth;
        }
        public void Register(AbstractClient client)
        {
            if (PlayerClients.Count < 2)
            {
                PlayerClients.Add(client);
                State = State switch
                {
                    ServerState.WaitingForBoth => ServerState.WaitingForOne,
                    _ => ServerState.WaitingForStart
                };
            }
        }
        public void Unregister(AbstractClient client)
        {
            if (PlayerClients.Remove(client))
            {
                State = State switch
                {
                    ServerState.WaitingForOne => ServerState.WaitingForBoth,
                    _ => ServerState.WaitingForOne
                };
            }
            OtherClients.Remove(client);
        }
        public void StartGame()
        {
            if (State == ServerState.WaitingForStart)
            {
                ServerPlayerCardSet[] cards = new ServerPlayerCardSet[2];
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        cards[i] = PlayerClients[i].RequestCardSet();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"编号为{i}的玩家的卡组无效！报错原因{ex.Message}");
                    }
                    if (!cards[i].Valid)
                    {
                        throw new Exception($"编号为{i}的玩家的卡组无效！可能的原因：检测到无效卡、数量不为3和30、携带了无法携带的卡。");
                    }
                }
                State = ServerState.Gaming;
                Game.StartGame(cards, PlayerClients);
            }
            else
            {
                throw new Exception("客户端还未完全就位,无法启动!");
            }
        }
        public void BroadCast(ClientUpdatePacket packet)
        {
            int teamID = packet.Category / 10;
            PlayerClients[teamID].Update(packet);
            PlayerClients[1 - teamID].Update(packet.Type switch
            {
                ClientUpdateType.Dice => new(ClientUpdateType.Dice, packet.Category, packet.Ints.Length),
                //使用卡牌的时候双方都能看到
                _ => packet
            });
        }
        public void BroadCastRegion() => PlayerClients.ForEach(c => c.UpdateRegion());
        public NetEvent RequestEvent(int playerid, OperationType demand)
        {
            PlayerClients[1 - playerid].RequestEnemyEvent(demand);
            return PlayerClients[playerid].RequestEvent(demand);
        }
    }
}
