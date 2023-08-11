//namespace TCGAI
//{
//    public class Game
//    {
//        internal void Init()
//        {
//            Mod.LoadAll(Directory.GetCurrentDirectory() + "/mods");

//            var now = DateTime.Now;
//            gameid = $"{now.Year,4}{now.Hour,2}{now.Minute,2}{now.Second,2}{now.Microsecond,3}";
//            Teams = new Team[2] { new(0, new DefaultAI(new GameInfo(0))), new(1, new PlayerAI(new GameInfo(1))) };
//        }
//    }
//    public class GameInfo : IGameInfo
//    {
//        private readonly int index;
//        internal GameInfo(int index) => this.index = index;
//        public TeamReadonly GetTeamReadonly() => Game.Instance.Teams[1 - index].ToReadonly();
//        public TeamDetailReadonly GetTeamDetailReadonly() => Game.Instance.Teams[index].ToDetailReadonly();
//    }
//}
