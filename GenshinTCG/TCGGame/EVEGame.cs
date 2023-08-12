using TCGClient;

namespace TCGGame
{
    internal class EVEGame : AbstractGame
    {
        public EVEGame() : base()
        {
            Clients = new[] { new BuiltInClient(), new BuiltInClient() };
            Array.ForEach(Clients, c => c.InitServerSetting(null));
        }
    }
}
