namespace TCGBase
{
    public partial class PlayerTeam
    {
        public Game Game { get; }
        public PlayerTeam Enemy => Game.Teams[1 - TeamID];
        /// <summary>
        /// 用于pvp模式仅限3个角色(然而设计模式似乎允许使用最多10个角色 Warning:未经测试)
        /// </summary>
        public Character[] Characters { get; protected init; }
        /// <summary>
        /// 在Game.Teams中的index
        /// </summary>
        public int TeamID { get; private init; }
        /// <summary>
        /// 只允许使用队内的random
        /// </summary>
        public CounterRandom Random { get; init; }
        public TeamSpecialState SpecialState { get; init; }
        public PersistentSet Supports { get; init; }
        public PersistentSet Summons { get; init; }
        public PersistentSet Effects { get; init; }
        private int _currcharacter;
        public int CurrCharacter
        {
            get => _currcharacter; internal set
            {
                _currcharacter = value;
                Game.BroadCast(ClientUpdateCreate.CharacterUpdate.SwitchUpdate(TeamID, value));
            }
        }
        public bool Pass { get; internal set; }

        internal List<AbstractCardAction> LeftCards { get; init; }
        internal CardsInHand CardsInHand { get; init; }
        /// <summary>
        /// max_size=16,默认顺序为 万能 冰水火雷岩草风(0-7)
        /// </summary>
        internal List<int> Dices { get; }
        public int LegendPoint { get; }
        /// <param name="cardset">经过处理确认正确的卡组</param>
        internal PlayerTeam(ServerPlayerCardSet cardset, Game game, int teamindex)
        {
            TeamID = teamindex;
            Game = game;

            Random = new();//TODO:SEED
            SpecialState = new();

            Summons = new(11, this, 4, false);
            Supports = new(12, this, 4, true);
            Effects = new(-1, this);

            Characters = cardset.CharacterCards.Select((c, i) => new Character(c, i, this)).ToArray();
            LeftCards = cardset.ActionCards.ToList();
            CardsInHand = new(this);

            Pass = false;
            Dices = new();

            _currcharacter = -1;
            LegendPoint = 1;
        }
        /// <summary>
        /// 回合开始时最先调用，如扔骰子等
        /// </summary>
        internal void RoundStart()
        {
            DiceRollingVariable drv = new();
            InstantTrigger(new SimpleSender(TeamID, SenderTag.RollingStart), drv);
            RollDice(drv);
            for (int i = 0; i < drv.RollingTimes; i++)
            {
                Game.RequestAndHandleEvent(TeamID, 30000, OperationType.ReRollDice);
            }
        }
        /// <summary>
        /// 回合结束时最后调用，清理骰子，抽2卡
        /// </summary>
        internal void RoundEnd()
        {
            Dices.Clear();
            RollCard(2);
        }
    }
}
