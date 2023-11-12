using System.Text.Json.Serialization;

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
        [JsonConstructor]
        public ReadonlyGame(int currTeam, int waitingTime, ReadonlyRegion me, int meID, List<int> dices, List<string> cards, int leftCardsNum, ReadonlyRegion enemy, int enemyDiceNum, int enemyCardNum, int enemyLeftCardsNum)
        {
            CurrTeam = currTeam;
            WaitingTime = waitingTime;
            Me = me;
            MeID = meID;
            Dices = dices;
            Cards = cards;
            LeftCardsNum = leftCardsNum;
            Enemy = enemy;
            EnemyDiceNum = enemyDiceNum;
            EnemyCardNum = enemyCardNum;
            EnemyLeftCardsNum = enemyLeftCardsNum;
        }
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
            Cards = teamMe.CardsInHand.Select(c => $"{c.Namespace}:{c.NameID}").ToList();

            EnemyDiceNum = teamEnemy.Dices.Count;
            EnemyCardNum = teamEnemy.CardsInHand.Count;
            EnemyLeftCardsNum = teamEnemy.LeftCards.Count;
        }
        /// <summary>
        /// 偷个懒
        /// </summary>
        public virtual void UpdateRegion(PlayerTeam teamMe, PlayerTeam teamEnemy)
        {
            Me = new(teamMe);
            Enemy = new(teamEnemy);
        }
        public virtual void Update(ClientUpdatePacket packet)
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
                        case 1://blend
                            if (teamID == MeID)
                            {
                                Cards.RemoveAt(packet.Ints[0]);
                            }
                            else
                            {
                                EnemyCardNum -= 1;
                            }
                            break;
                        case 2://obtain
                            if (teamID == MeID)
                            {
                                Cards.Add(packet.Strings[0]);
                            }
                            else
                            {
                                EnemyCardNum += 1;
                            }
                            break;
                        case 3://push
                            int cnt = packet.Ints.Length;
                            if (teamID == MeID)
                            {
                                LeftCardsNum += cnt;
                                //转为从大到小
                                foreach (var i in packet.Ints.Reverse())
                                {
                                    Cards.RemoveAt(i);
                                }
                            }
                            else
                            {
                                EnemyLeftCardsNum += cnt;
                                EnemyCardNum -= cnt;
                            }
                            break;
                        case 4://pop
                            if (teamID == MeID)
                            {
                                LeftCardsNum -= 1;
                            }
                            else
                            {
                                EnemyLeftCardsNum -= 1;
                            }
                            break;
                            //broke:do nothing
                    }
                    break;
                default:
                    throw new NotImplementedException("ReadonlyGame.Update():收到了莫名其妙的packet!");
            }
        }
    }
}
