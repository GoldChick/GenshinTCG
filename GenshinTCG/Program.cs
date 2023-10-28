using TCGBase;
using TCGClient;

namespace GenshinTCG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var now = DateTime.Now;
            string gameid = $"{now.Year,4}{now.Hour,2}{now.Minute,2}{now.Second,2}{now.Microsecond,3}";

            Game game = new();
            var c0 = new ConsoleClient();
            var c1 = new SleepClient();

            c0.InitServerSetting(null);

            game.AddClient(c0);
            game.AddClient(c1);

            game.StartGame();
        }
    }
}