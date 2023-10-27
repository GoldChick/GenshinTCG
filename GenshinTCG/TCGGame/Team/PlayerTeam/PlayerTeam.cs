using System.Text.Json;
using TCGBase;
using TCGClient;
using TCGUtil;

namespace TCGGame
{
    public partial class PlayerTeam : IPrintable
    {
        internal AbstractGame Game { get; private init; }
        /// <summary>
        /// 在Game.Teams中的index
        /// </summary>
        internal int TeamIndex { get; private init; }
        internal PlayerTeam Enemy => Game.Teams[1 - TeamIndex];
        internal AbstractClient Client { get => Game.Clients[TeamIndex]; }
        /// <summary>
        /// TODO:用于:TODO
        /// </summary>
        public bool IsPreviewMode { get; }
        /// <summary>
        /// 只允许使用队内的random
        /// </summary>
        internal CounterRandom Random { get; init; }
        /// <summary>
        /// 用于pvp模式仅限4个角色(NOTE:pve-很远的将来)
        /// </summary>
        public Character[] Characters { get; protected init; }


        public PersistentSet<AbstractCardPersistentSupport> Supports { get; init; }
        public PersistentSet<AbstractCardPersistentSummon> Summons { get; init; }
        public PersistentSet<AbstractCardPersistentEffect> Effects { get; init; }

        public int CurrCharacter { get; internal set; }
        public bool Pass { get; internal set; }

        internal List<ActionCard> LeftCards { get; init; }
        public List<ActionCard> CardsInHand { get; init; }
        /// <summary>
        /// max_size=16,默认顺序为 万能 冰水火雷岩草风(0-7)
        /// </summary>
        internal List<int> Dices { get; }
        public PlayerTeam(AbstractGame game, int index)
        {
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
        /// <param name="cardset">经过处理确认正确的卡组</param>
        public PlayerTeam(ServerPlayerCardSet cardset, AbstractGame game, int index) : this(game, index)
        {
            Characters = cardset.CharacterCards.Select((c, i) => new Character(c, i)).ToArray();
            LeftCards = cardset.ActionCards.Select(a => new ActionCard(a)).ToList();
            CardsInHand = new();
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ReplaceAssist()
        {
            return false;
        }
        /// <summary>
        /// 回合开始时最先调用，如扔骰子等
        /// </summary>
        public virtual void RoundStart()
        {
            Logger.Print("team:start");
            DiceRollingVariable v = new();
            Game.EffectTrigger(new SimpleSender(TeamIndex, SenderTag.RollingStart), v);
            RollDice(v);
            for (int i = 0; i < v.RollingTimes; i++)
            {
                Game.RequestAndHandleEvent(TeamIndex, 30000, ActionType.ReRollDice, "reroll_dice?");
            }
        }
        /// <summary>
        /// 回合结束时最后调用，如清理骰子等
        /// </summary>
        public virtual void RoundEnd()
        {
            Dices.Clear();
            RollCard(2);
        }
        public virtual void Print()
        {
            Logger.Print($"Index:{TeamIndex}");
            Logger.Print($"Pass:{Pass}");
            Logger.Print($"DiceNum:{Dices.Count}");
            Logger.Print("----骰子种类----：万能冰水火雷岩草风");
            Logger.Print($"现在拥有的骰子数： {JsonSerializer.Serialize(GetDices())}");
            Logger.Print($"CurrCharacter:{CurrCharacter}");
            Array.ForEach(Characters, c => c.Print());
            Logger.Print($"Team Effects:");
            Effects.Print();
            Logger.Print($"Supports:");
            Supports.Print();
            Logger.Print($"Summons:");
            Summons.Print();
            CardsInHand.ForEach(c => c.Print());
            Logger.Print($"");
        }
    }
}
