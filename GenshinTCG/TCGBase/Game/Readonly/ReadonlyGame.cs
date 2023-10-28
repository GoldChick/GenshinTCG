using System.Text.Json;
using TCGClient;
using TCGUtil;

namespace TCGBase
{
    /// <summary>
    /// 从一个玩家的视角展现的
    /// </summary>
    public class ReadonlyGame
    {
        public int CurrTeam { get; private set; }
        //TODO:@desperated
        public int WaitingTime { get; private set; }
        public ReadonlyRegion Me { get; private set; }
        public int MeID { get; }
        public List<int> Dices { get; }
        public List<string> Cards { get; }
        public int LeftCardsNum { get; private set; }

        public ReadonlyRegion Enemy { get; private set; }
        public int EnemyDiceNum { get; private set; }
        public int EnemyCardNum { get; private set; }
        public int EnemyLeftCardsNum { get; private set; }
        public ReadonlyGame(Game game, int me)
        {
            CurrTeam = game.CurrTeam;
            WaitingTime = 114514;
            MeID = me;

            var teamMe = game.Teams[me];
            var teamEnemy = game.Teams[1 - me];

            Me = new(teamMe);
            Enemy = new(teamEnemy);

            LeftCardsNum = teamMe.LeftCards.Count;
            Dices = teamMe.Dices.ToList();
            Cards = teamMe.CardsInHand.Select(c => c.Card.NameID).ToList();

            EnemyDiceNum = teamEnemy.Dices.Count;
            EnemyCardNum = teamEnemy.CardsInHand.Count;
            EnemyLeftCardsNum = teamEnemy.LeftCards.Count;
        }
        /// <summary>
        /// 偷个懒
        /// </summary>
        public void UpdateRegion(PlayerTeam teamMe, PlayerTeam teamEnemy)
        {
            Me = new(teamMe);
            Enemy = new(teamEnemy);
        }
        public void Update(ClientUpdatePacket packet)
        {
            int teamID = packet.Category / 10;
            int category = packet.Category % 10;

            switch (packet.Type)
            {
                case ClientUpdateType.CurrTeam:
                    CurrTeam = packet.Ints[0];
                    break;
                //TODO:有必要吗
                case ClientUpdateType.WaitingTime:
                    WaitingTime = packet.Ints[0];
                    break;
                case ClientUpdateType.Character:
                    //TODO
                    break;
                case ClientUpdateType.Persistent:
                    //TODO
                    break;
                case ClientUpdateType.Dice:
                    {
                        if (teamID == MeID)
                        {
                            Dices.Clear();
                            Dices.AddRange(packet.Ints);
                        }
                        else
                        {
                            EnemyDiceNum = packet.Ints[0];
                        }
                    }
                    break;
                case ClientUpdateType.Card:
                    switch (category)
                    {
                        case 0://use
                        case 1://consume
                            if (teamID == MeID)
                            {
                                Array.ForEach(packet.Strings, s => Cards.RemoveAt(Cards.FindIndex(c => c == s)));
                            }
                            else
                            {
                                EnemyCardNum -= packet.Ints[0];
                            }
                            if (category == 0)
                            {
                                //TODO:use的表现
                                //Logger.Print($"使用了卡牌{JsonSerializer.Serialize(packet.Strings)}");
                            }
                            break;
                        case 2://obtain
                            if (teamID == MeID)
                            {
                                Array.ForEach(packet.Strings, Cards.Add);
                            }
                            else
                            {
                                EnemyCardNum += packet.Ints[0];
                            }
                            break;
                        case 3://push
                        case 4://pop
                            int cnt = packet.Strings.Length;
                            if (teamID == MeID)
                            {
                                if (category == 3)
                                {
                                    LeftCardsNum += cnt;
                                    Array.ForEach(packet.Strings, s => Cards.RemoveAt(Cards.FindIndex(c => c == s)));
                                }
                                else
                                {
                                    LeftCardsNum -= cnt;
                                }
                            }
                            else
                            {
                                EnemyLeftCardsNum += cnt * (7 - category * 2);
                            }
                            break;
                        default:
                            throw new NotImplementedException("ReadonlyGame.Update():收到了莫名其妙的packet!");
                    }
                    break;
                default:
                    throw new NotImplementedException("ReadonlyGame.Update():收到了莫名其妙的packet!");
            }
        }
    }
}
