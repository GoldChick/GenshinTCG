using System.Text.Json;

namespace TCGBase
{
    internal class RegistryFromJson
    {
        public JsonSerializerOptions JsonOptionCard { get; }
        public JsonSerializerOptions JsonOptionTriggerable { get; }
        public RegistryFromJson()
        {
            JsonOptionCard = new()
            {
                Converters = { new JsonConverterCard(), new JsonConverterSelect(), new JsonConverterCondition() }
            };
            JsonOptionTriggerable = new()
            {
                Converters = { new JsonConverterTriggerable() }
            };
        }
        public CardCharacter CreateCharacterCard(string json)
        {
            try
            {
                CardRecordCharacter? record = JsonSerializer.Deserialize<CardRecordCharacter>(json);
                if (record != null)
                {
                    return new(record);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            throw new Exception("sb?");
        }
        public AbstractCardBase CreateActionCard(string json)
        {
            try
            {
                CardRecordBase? record = JsonSerializer.Deserialize<CardRecordBase>(json, JsonOptionCard);
                if (record != null)
                {
                    return record.GetCard();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            throw new Exception("sb?");
        }
        public ITriggerable CreateTriggerable(string json)
        {
            try
            {
                TriggerableRecordBase? rb = JsonSerializer.Deserialize<TriggerableRecordBase>(json, JsonOptionTriggerable);
                if (rb != null)
                {
                    return rb.GetTriggerable();
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
