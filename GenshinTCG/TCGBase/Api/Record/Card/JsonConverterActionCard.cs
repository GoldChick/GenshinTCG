using System.Text.Json;
using System.Text.Json.Serialization;

namespace TCGBase
{
    public class JsonConverterActionCard : JsonConverter<CardRecordAction>
    {
        public override CardRecordAction? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            if (root.TryGetProperty("CardType", out JsonElement typeElement))
            {
                return typeElement.GetString() switch
                {
                    //TODO: summon and effect
                    //"Summon" or "Effect" => JsonSerializer.Deserialize<CardRecordEffect>(root.GetRawText(), options),
                    "Equipment" => JsonSerializer.Deserialize<CardRecordEquipment>(root.GetRawText(), options),
                    "Support" => JsonSerializer.Deserialize<CardRecordSupport>(root.GetRawText(), options),
                    "Event" => JsonSerializer.Deserialize<CardRecordEvent>(root.GetRawText(), options),
                    _ => throw new JsonException("JsonConverterCard.Read() : Not Registered 'CardType' property."),
                };
            }
            throw new JsonException("JsonConverterCard.Read() : Missing or invalid 'CardType' property.(NOT Ignore Case)");
        }

        public override void Write(Utf8JsonWriter writer, CardRecordAction value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
