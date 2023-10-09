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
        public ReadonlyGame()
        {
            Me = new();
            Enemy = new();

            Dices = new();
            Cards = new();
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
    public class ReadonlyRegion
    {
        public List<ReadonlyCharacter> Characters { get; }
        public List<ReadonlyPersistent> Effects { get; }
        public List<ReadonlyPersistent> Summons { get; }
        public List<ReadonlyPersistent> Supports { get; }
        public ReadonlyRegion()
        {
            Characters = new();
            Effects = new();
            Summons = new();
            Supports = new();
        }
    }
    public abstract class AbstractReadonlyObject
    {
        public string Name { get; }
        public AbstractReadonlyObject(string name)
        {
            Name = name;
        }
    }
    public class ReadonlyCard : AbstractReadonlyObject
    {
        public ReadonlyCard(string name) : base(name)
        {
        }
    }
    public class ReadonlyCharacter : AbstractReadonlyObject
    {
        public int HP { get; set; }
        public int MP { get; set; }
        public int Element { get; set; }
        public ReadonlyCharacter(string name) : base(name)
        {
        }
    }
    public class ReadonlyPersistent : AbstractReadonlyObject
    {
        //TODO: get a name?
        public int[] Ints { get; set; }
        public ReadonlyPersistent(string name) : base(name)
        {
        }
    }

}
