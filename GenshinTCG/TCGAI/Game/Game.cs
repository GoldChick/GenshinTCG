using TCGUtil;
using Program;
using TCGConsole;
using TCGGame;
using TCGRecord;

namespace TCGAI
{
    public class Game
    {
        private static Game? _instance = new();
        private GameStage _stage = GameStage.After;
        internal static Game Instance
        {
            get
            {
                _instance ??= new();
                return _instance;
            }
        }
        public static readonly int MAX_THINK_TIME = 10500;
        public string gameid;
        internal Team[] Teams { get; set; }
        internal GameStage Stage
        {
            get => _stage;
            set
            {
                //TODO:DO SOMETHING
                _stage = value;
            }
        }
        internal void Init()
        {
            Mod.LoadAll(Directory.GetCurrentDirectory() + "/mods");

            var now = DateTime.Now;
            gameid = $"{now.Year,4}{now.Hour,2}{now.Minute,2}{now.Second,2}{now.Microsecond,3}";
            Teams = new Team[2] { new(0, new DefaultAI(new GameInfo(0))), new(1, new PlayerAI(new GameInfo(1))) };
        }
    }
    public class GameInfo : IGameInfo
    {
        private readonly int index;
        internal GameInfo(int index) => this.index = index;
        public TeamReadonly GetTeamReadonly() => Game.Instance.Teams[1 - index].ToReadonly();
        public TeamDetailReadonly GetTeamDetailReadonly() => Game.Instance.Teams[index].ToDetailReadonly();
    }
}
