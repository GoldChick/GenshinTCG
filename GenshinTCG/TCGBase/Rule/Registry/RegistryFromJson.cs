using System.Text.Json;

namespace TCGBase
{
    internal static class RegistryFromJson
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
        public static AbstractCardBase CreateActionCard(string json)
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
                        case CardType.Effect:
                            var effect = JsonSerializer.Deserialize<CardRecordEffect>(json);
                            if (effect!=null)
                            {
                                return new CardEffect(effect);
                            }
                            break;
                        case CardType.Equipment:
                            var equip = JsonSerializer.Deserialize<CardRecordEquipment>(json);
                            if (equip != null)
                            {
                                return new CardEquipment(equip);
                            }
                            break;
                        case CardType.Support:
                            var sup = JsonSerializer.Deserialize<CardRecordSupport>(json);
                            if (sup != null)
                            {
                                return new CardSupport(sup);
                            }
                            break;
                        case CardType.Event:
                            var evt = JsonSerializer.Deserialize<CardRecordEvent>(json);
                            if (evt != null)
                            {
                                return new CardEvent(evt);
                            }
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
                                return new TriggerableSkill(skill);
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
