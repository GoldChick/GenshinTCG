using System.Text.Json;
using TCGClient;
using TCGUtil;

namespace TCGGame
{
    /// <summary>
    /// 从一个玩家的视角展现的
    /// </summary>
    public class ReadonlyGame
    {
        public int CurrTeam { get; set; }
        //TODO:@desperated
        public int WaitingTime { get; set; }
        public ReadonlyRegion Me { get; }
        public int MeID { get; }
        public List<int> Dices { get; }
        public List<ReadonlyCard> Cards { get; }
        public int LeftCardsNum { get; set; }

        public ReadonlyRegion Enemy { get; }
        public int EnemyDiceNum { get; set; }
        public int EnemyCardNum { get; set; }
        public int EnemyLeftCardsNum { get; set; }
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
            Cards = teamMe.CardsInHand.Select(c => new ReadonlyCard(c.Card.NameID)).ToList();

            EnemyDiceNum = teamEnemy.Dices.Count;
            EnemyCardNum = teamEnemy.CardsInHand.Count;
            EnemyLeftCardsNum = teamEnemy.LeftCards.Count;
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
                    switch (category)
                    {
                        case 0://consume
                            if (teamID == MeID)
                            {
                                Array.ForEach(packet.Ints, d => Dices.Remove(d));
                            }
                            else
                            {
                                EnemyDiceNum -= packet.Ints[0];
                            }
                            break;
                        case 1://obtain
                            if (teamID == MeID)
                            {
                                Dices.AddRange(packet.Ints);
                            }
                            else
                            {
                                EnemyDiceNum -= packet.Ints[0];
                            }
                            break;
                        default:
                            throw new NotImplementedException("ReadonlyGame.Update():收到了莫名其妙的packet!");
                    }
                    break;
                case ClientUpdateType.Card:
                    switch (category)
                    {
                        case 0://use
                            //TODO
                            Logger.Print($"使用了卡牌{JsonSerializer.Serialize(packet.Strings)}");
                            break;
                        case 1://consume
                            if (teamID == MeID)
                            {
                                Array.ForEach(packet.Strings, s => Cards.RemoveAt(Cards.FindIndex(c => c.Name == s)));
                            }
                            else
                            {
                                EnemyCardNum -= packet.Ints[0];
                            }
                            break;
                        case 2://obtain
                            if (teamID == MeID)
                            {
                                Array.ForEach(packet.Strings, s => Cards.Add(new(s)));
                            }
                            else
                            {
                                EnemyCardNum += packet.Ints[0];
                            }
                            break;
                        case 3://push
                        case 4://pop
                            if (teamID == MeID)
                            {
                                LeftCardsNum += packet.Ints[0] * (7 - category * 2);
                            }
                            else
                            {
                                EnemyLeftCardsNum += packet.Ints[0] * (7 - category * 2);
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
