using TCGClient;

namespace TCGGame
{
    internal class PVPGame : AbstractGame
    {
        public PVPGame() : base()
        {
            Clients = new[] { new BuiltInClient(), new BuiltInClient() };
            Array.ForEach(Clients, c => c.InitServerSetting(null));
        }
    }
}
