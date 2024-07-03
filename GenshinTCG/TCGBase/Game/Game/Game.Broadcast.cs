namespace TCGBase
{
    public partial class Game
    {
        public AbstractServer Server { get; }
        public void BroadCast(ClientUpdatePacket packet) => Server.BroadCast(packet);
        public void BroadCastRegion() => Server.BroadCastRegion();
    }
}
