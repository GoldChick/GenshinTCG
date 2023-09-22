using TCGGame;

namespace GenshinTCG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var now = DateTime.Now;
            string gameid = $"{now.Year,4}{now.Hour,2}{now.Minute,2}{now.Second,2}{now.Microsecond,3}";

            
            PVPGame game = new();
            game.StartGame();
        }
    }
}