using System.Text.Json;

namespace TCGBase
{
    //TODO check it
    internal static class RegistryFromJson
    {
        public static CardCharacter CreateCharacterCard(string json)
        {
            try
            {
                CharacterCardRecord? record = JsonSerializer.Deserialize<CharacterCardRecord>(json);
                if (record != null)
                {
                    CardCharacter c = new(record);
                    return c;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            throw new Exception("sb?");
        }
        public static void CreateActionCard(string json)
        {
            try
            {
                ActionCardRecord? record = JsonSerializer.Deserialize<ActionCardRecord>(json);
                if (record != null)
                {
                    switch (record.CardType)
                    {
                        case CardType.Character:
                            throw new ArgumentException($"为什么你要往行动卡里注册事件？？注册的nameid为{record.NameID}");
                        case CardType.Summon:
                            break;
                        case CardType.Equipment:
                            break;
                        case CardType.Support:
                            break;
                        case CardType.Event:
                            break;
                        case CardType.Effect:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            throw new Exception("sb?");
        }
    }
}
