using TCGClient;

namespace TCGGame
{
    internal class PVPGame : AbstractGame
    {
        public PVPGame() : base()
        {
            var c0 = new BuiltInClient();
            var c1 = new BuiltInClient();

            c0.InitServerSetting(null);
            c1.InitServerSetting(null, 1);

            Clients = new[] { c0, c1 };}
    }
}
