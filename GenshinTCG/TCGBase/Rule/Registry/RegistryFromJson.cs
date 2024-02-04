using System.Text.Json;

namespace TCGBase
{
    //TODO: public for test
    public static class RegistryFromJson
    {
        public static CardCharacter CreateCharacterCard(string json)
        {
            try
            {
                CardRecordCharacter? record = JsonSerializer.Deserialize<CardRecordCharacter>(json);
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
                CardRecordAction? record = JsonSerializer.Deserialize<CardRecordAction>(json);
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
        public static ITriggerable CreateTriggerable(string json)
        {
            try
            {
                TriggerableRecordBase? rb = JsonSerializer.Deserialize<TriggerableRecordBase>(json);
                if (rb != null)
                {
                    switch (rb.Type)
                    {
                        case TriggerableType.Skill:
                            TriggerableRecordSkill? skill = JsonSerializer.Deserialize<TriggerableRecordSkill>(json);
                            if (skill != null)
                            {
                                return new SkillTriggerable(skill);
                            }
                            break;
                        case TriggerableType.AfterUseSkill:
                            break;
                        case TriggerableType.AfterUseCard:
                            break;
                        case TriggerableType.AfterBlend:
                            break;
                        case TriggerableType.AfterSwitch:
                            break;
                        case TriggerableType.AfterHurt:
                            break;
                        case TriggerableType.AfterHealed:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            throw new Exception("RegistryFromJson:Out Of TriggerableType!");
        }
    }
}
