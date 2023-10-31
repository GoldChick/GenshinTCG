namespace TCGBase
{
    public partial class Game
    {
        public void BroadCast(ClientUpdatePacket packet)
        {
            int teamID = packet.Category / 10;
            Clients[teamID].Update(packet);
            Clients[1 - teamID].Update(packet.Type switch
            {
                ClientUpdateType.Dice => new(ClientUpdateType.Dice, packet.Category, packet.Ints.Length),
                //使用卡牌的时候双方都能看到
                _ => packet
            });
        }
        /// <summary>
        /// TODO：偷个懒
        /// </summary>
        public void BroadCastRegion()
        {
            Clients[0].UpdateRegion();
            Clients[1].UpdateRegion();
        }
    }
}
