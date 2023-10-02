using TCGClient;

namespace TCGGame
{
    public abstract partial class AbstractGame
    {
        public void BroadCast(ClientUpdatePacket packet)
        {
            //ClientUpdatePacket readonlyPacket = packet.Type switch
            //{
            //    ClientUpdateType.Dice => new(ClientUpdateType.Dice, packet.Category, packet.Ints.Length),
            //    ClientUpdateType.Card => new(ClientUpdateType.Card, packet.Category, packet.Strings.Length),
            //    _ => packet
            //};
            int teamID = packet.Category / 10;
            Clients[teamID].Update(packet);
            Clients[1 - teamID].Update(packet.Type switch
            {
                ClientUpdateType.Dice => new(ClientUpdateType.Dice, packet.Category, packet.Ints.Length),
                ClientUpdateType.Card => new(ClientUpdateType.Card, packet.Category, packet.Strings.Length),
                _ => packet
            });
        }
    }
}
