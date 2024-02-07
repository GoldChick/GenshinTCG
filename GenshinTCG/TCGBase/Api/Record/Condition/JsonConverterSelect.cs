using System.Text.Json;
using System.Text.Json.Serialization;

namespace TCGBase
{
    public class JsonConverterCondition : JsonConverter<ConditionRecordBase>
    {
        public override ConditionRecordBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            if (root.TryGetProperty("Type", out JsonElement typeElement))
            {
                return typeElement.GetString() switch
                {
                    "Counter" => JsonSerializer.Deserialize<ConditionRecordCounter>(root.GetRawText(), options),
                    "Effect" => JsonSerializer.Deserialize<ConditionRecordEffect>(root.GetRawText(), options),
                    _ => throw new JsonException("JsonConverterCondition.Read() : Not Registered 'Type' property."),
                };
            }
            throw new JsonException("JsonConverterCondition.Read() : Missing or invalid 'Type' property.(NOT Ignore Case)");
        }

        public override void Write(Utf8JsonWriter writer, ConditionRecordBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
