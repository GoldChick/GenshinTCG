namespace TCGBase
{
    public partial class PlayerTeam : AbstractTeam
    {
        public override AbstractGame Game { get => RealGame; }
        internal Game RealGame { get; }
        public override PlayerTeam Enemy => RealGame.Teams[1 - TeamIndex];
        /// <summary>
        /// 用于pvp模式仅限3个角色(然而设计模式似乎允许使用最多10个角色 Warning:未经测试)
        /// </summary>
        public override Character[] Characters { get; protected init; }

        private int _currcharacter;
        public override int CurrCharacter
        {
            get => _currcharacter; internal set
            {
                _currcharacter = value;
                RealGame.BroadCast(ClientUpdateCreate.CharacterUpdate.SwitchUpdate(TeamIndex, value));
            }
        }
        public bool Pass { get; internal set; }

        internal List<AbstractCardAction> LeftCards { get; init; }
        //internal List<AbstractCardAction> CardsInHand { get; init; }
        internal CardsInHand CardsInHand { get; init; }
        /// <summary>
        /// max_size=16,默认顺序为 万能 冰水火雷岩草风(0-7)
        /// </summary>
        internal List<int> Dices { get; }
        /// <param name="cardset">经过处理确认正确的卡组</param>
        internal PlayerTeam(ServerPlayerCardSet cardset, Game game, int teamindex) : base(teamindex)
        {
            RealGame = game;

            Characters = cardset.CharacterCards.Select((c, i) => new Character(c, i, this)).ToArray();
            LeftCards = cardset.ActionCards.ToList();
            CardsInHand = new();

            Pass = false;
            Dices = new();


            _currcharacter = -1;
        }
        /// <summary>
        /// 回合开始时最先调用，如扔骰子等
        /// </summary>
        internal void RoundStart()
        {
            DiceRollingVariable drv = new();
            EffectTrigger(new SimpleSender(TeamIndex, SenderTag.RollingStart), drv);
            RollDice(drv);
            for (int i = 0; i < drv.RollingTimes; i++)
            {
                RealGame.RequestAndHandleEvent(TeamIndex, 30000, ActionType.ReRollDice);
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
        internal EmptyTeam ToEmpty(Character die)
        {
            return new EmptyTeam(die, this);
        }
    }
}
