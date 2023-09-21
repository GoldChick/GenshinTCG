using System.Text.Json;
using TCGBase;
using TCGUtil;

namespace TCGGame
{
    public partial class PlayerTeam : AbstractTeam
    {
        internal List<ActionCard> LeftCards { get; init; }
        public List<ActionCard> CardsInHand { get; init; }
        /// <summary>
        /// max_size=16,默认顺序为 万能 冰水火雷岩草风(0-7)
        /// </summary>
        protected List<int> Dices { get; } = new();

        /// <param name="cardset">经过处理确认正确的卡组</param>
        public PlayerTeam(ServerPlayerCardSet cardset, AbstractGame game, int index) : base(game, index)
        {
            UseDice = true;
            Characters = cardset.CharacterCards.Select(c => new Character(c)).ToArray();
            LeftCards = cardset.ActionCards.Select(a => new ActionCard(a)).ToList();
            CardsInHand = new();
        }


        public override void RoundStart()
        {
            Logger.Print("team:start");
            DiceRollingVariable v = new(TeamIndex);
            Game.EffectTrigger(new SimpleSender(Tags.SenderTags.ROLLING_START), v);
            RollDice(v);
        }
        public override void RoundEnd()
        {
            Dices.Clear();
            RollCard(2);
        }
        public override void Print()
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
            CardsInHand.ForEach( c => c.Print());
            Logger.Print($"");
        }
    }

}
