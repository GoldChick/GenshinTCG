namespace TCGBase
{
    public partial class PlayerTeam
    {
        public Game Game { get; private init; }
        /// <summary>
        /// 在Game.Teams中的index
        /// </summary>
        public int TeamIndex { get; private init; }
        public PlayerTeam Enemy => Game.Teams[1 - TeamIndex];
        /// <summary>
        /// 只允许使用队内的random
        /// </summary>
        public CounterRandom Random { get; init; }
        /// <summary>
        /// 用于pvp模式仅限3个角色(然而设计模式似乎允许使用最多10个角色 Warning:未经测试)
        /// </summary>
        public Character[] Characters { get; protected init; }


        public PersistentSet<AbstractCardPersistent> Supports { get; init; }
        public PersistentSet<AbstractCardPersistentSummon> Summons { get; init; }
        public PersistentSet<AbstractCardPersistentEffect> Effects { get; init; }

        public int CurrCharacter { get; internal set; }
        public bool Pass { get; internal set; }

        internal List<AbstractCardAction> LeftCards { get; init; }
        internal List<AbstractCardAction> CardsInHand { get; init; }
        /// <summary>
        /// max_size=16,默认顺序为 万能 冰水火雷岩草风(0-7)
        /// </summary>
        internal List<int> Dices { get; }
        /// <param name="cardset">经过处理确认正确的卡组</param>
        public PlayerTeam(ServerPlayerCardSet cardset, Game game, int index)
        {
            Characters = cardset.CharacterCards.Select((c, i) => new Character(c, i, this)).ToArray();
            LeftCards = cardset.ActionCards.ToList();
            CardsInHand = new();

            CurrCharacter = -1;
            Pass = false;
            Dices = new();
            Random = new();//TODO:SEED

            Supports = new(12, 4, true);
            Summons = new(11, 4);
            Effects = new(-1);

            Game = game;
            TeamIndex = index;
        }
        /// <summary>
        /// 回合开始时最先调用，如扔骰子等
        /// </summary>
        public virtual void RoundStart()
        {
            DiceRollingVariable drv = new();
            EffectTrigger(Game, TeamIndex, new SimpleSender(TeamIndex, SenderTag.RollingStart), drv);
            RollDice(drv);
            for (int i = 0; i < drv.RollingTimes; i++)
            {
                Game.RequestAndHandleEvent(TeamIndex, 30000, ActionType.ReRollDice);
            }
        }
        /// <summary>
        /// 回合结束时最后调用，清理骰子，抽2卡
        /// </summary>
        public virtual void RoundEnd()
        {
            Dices.Clear();
            RollCard(2);
        }
    }
}
