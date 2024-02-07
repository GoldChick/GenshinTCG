using System.Text.Json;
using System.Text.Json.Serialization;

namespace TCGBase
{
    public class JsonConverterTriggerable : JsonConverter<TriggerableRecordBase>
    {
        public override TriggerableRecordBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            if (root.TryGetProperty("Type", out JsonElement typeElement))
            {
                return typeElement.GetString() switch
                {
                    "Skill" => JsonSerializer.Deserialize<TriggerableRecordSkill>(root.GetRawText(), options),
                    "Card" => JsonSerializer.Deserialize<TriggerableRecordCard>(root.GetRawText(), options),
                    _ => throw new JsonException("JsonConverterTriggerable.Read() : Not Registered 'Type' property."),
                };
            }
            throw new JsonException("JsonConverterTriggerable.Read() : Missing or invalid 'Type' property.(NOT Ignore Case)");
        }

        public override void Write(Utf8JsonWriter writer, TriggerableRecordBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
