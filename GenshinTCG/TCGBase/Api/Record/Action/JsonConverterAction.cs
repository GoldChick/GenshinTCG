using System.Text.Json;
using System.Text.Json.Serialization;

namespace TCGBase
{
    public class JsonConverterAction : JsonConverter<ActionRecordBase>
    {
        public override ActionRecordBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            if (root.TryGetProperty("Type", out JsonElement typeElement))
            {
                return typeElement.GetString() switch
                {
                    "MP" => JsonSerializer.Deserialize<ActionRecordMP>(root.GetRawText(), options),
                    "Damage" => JsonSerializer.Deserialize<ActionRecordDamage>(root.GetRawText(), options),
                    "Dice" => JsonSerializer.Deserialize<ActionRecordDice>(root.GetRawText(), options),
                    "Effect" => JsonSerializer.Deserialize<ActionRecordEffect>(root.GetRawText(), options),
                    "Heal" => JsonSerializer.Deserialize<ActionRecordHeal>(root.GetRawText(), options),
                    _ => throw new JsonException($"Unregistered ActionRecord 'Type' property: {typeElement}."),
                };
            }
            throw new JsonException("JsonConverterAction.Read() : Missing or invalid 'CardType' property.(NOT Ignore Case)");
        }

        public override void Write(Utf8JsonWriter writer, ActionRecordBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
