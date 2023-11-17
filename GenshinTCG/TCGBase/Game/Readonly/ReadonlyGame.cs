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
        public List<ReadonlyObject> Cards { get; }
        public int LeftCardsNum { get; private set; }
        public ReadonlyRegion Enemy { get; private set; }
        public int EnemyDiceNum { get; private set; }
        public int EnemyCardNum { get; private set; }
        public int EnemyLeftCardsNum { get; private set; }
        [JsonConstructor]
        public ReadonlyGame(int currTeam, int waitingTime, ReadonlyRegion me, int meID, List<int> dices, List<ReadonlyObject> cards, int leftCardsNum, ReadonlyRegion enemy, int enemyDiceNum, int enemyCardNum, int enemyLeftCardsNum)
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
            Cards = teamMe.CardsInHand.Select(c => new ReadonlyObject(c.Namespace, c.NameID)).ToList();

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

            var packetteam = new ReadonlyRegion[] { Me, Enemy }[int.Abs(MeID - teamID)];
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
                    var charactercatagory = (ClientUpdateCreate.CharacterUpdateCategory)category;
                    switch (charactercatagory)
                    {
                        case ClientUpdateCreate.CharacterUpdateCategory.Hurt:
                            packetteam.Characters[packet.Ints[0]].HP -= packet.Ints[2];
                            break;
                        case ClientUpdateCreate.CharacterUpdateCategory.Heal:
                            packetteam.Characters[packet.Ints[0]].HP += packet.Ints[1];
                            break;
                        case ClientUpdateCreate.CharacterUpdateCategory.ChangeElement:
                            packetteam.Characters[packet.Ints[0]].Element = packet.Ints[1];
                            break;
                        case ClientUpdateCreate.CharacterUpdateCategory.Die:
                            //TODO: die ?
                            break;
                        case ClientUpdateCreate.CharacterUpdateCategory.UseSkill:
                            //TODO: below
                            break;
                        case ClientUpdateCreate.CharacterUpdateCategory.Switch:
                            packetteam.CurrCharacter = packet.Ints[0];
                            break;
                        default:
                            break;
                    }
                    break;
                case ClientUpdateType.Persistent:
                    var persistentcategory = (ClientUpdateCreate.PersistentUpdateCategory)category;
                    var effects = packet.Ints[0] switch
                    {
                        -1 => packetteam.Effects,
                        11 => packetteam.Summons,
                        12 => packetteam.Supports,
                        _ => packetteam.Characters[packet.Ints[0]].Effects //-1
                    };
                    switch (persistentcategory)
                    {
                        case ClientUpdateCreate.PersistentUpdateCategory.Obtain:
                            effects.Add(new(packet.Strings[0], packet.Strings[1], packet.Ints[1], packet.Ints[2]));
                            break;
                        case ClientUpdateCreate.PersistentUpdateCategory.Trigger:
                            effects[packet.Ints[1]].AvailableTimes = packet.Ints[2];
                            break;
                        case ClientUpdateCreate.PersistentUpdateCategory.Lose:
                            effects.RemoveAt(packet.Ints[1]);
                            break;
                    }
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
                    var cardcategory = (ClientUpdateCreate.CardUpdateCategory)category;
                    switch (cardcategory)
                    {
                        case ClientUpdateCreate.CardUpdateCategory.Use:
                        case ClientUpdateCreate.CardUpdateCategory.Blend:
                            if (teamID == MeID)
                            {
                                Cards.RemoveAt(packet.Ints[0]);
                            }
                            else
                            {
                                EnemyCardNum -= 1;
                            }
                            break;
                        case ClientUpdateCreate.CardUpdateCategory.Obtain:
                            if (teamID == MeID)
                            {
                                Cards.Add(new ReadonlyObject(packet.Strings[0], packet.Strings[1]));
                            }
                            else
                            {
                                EnemyCardNum += 1;
                            }
                            break;
                        case ClientUpdateCreate.CardUpdateCategory.Push:
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
                        case ClientUpdateCreate.CardUpdateCategory.Pop:
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
