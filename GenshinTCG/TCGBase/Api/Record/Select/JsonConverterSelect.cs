using System.Text.Json;
using System.Text.Json.Serialization;

namespace TCGBase
{
    public class JsonConverterSelect : JsonConverter<SelectRecordBase>
    {
        public override SelectRecordBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            if (root.TryGetProperty("Type", out JsonElement typeElement))
            {
                return typeElement.GetString() switch
                {
                    "Character" => JsonSerializer.Deserialize<SelectRecordCharacter>(root.GetRawText(), options),
                    "Summon" => JsonSerializer.Deserialize<SelectRecordSummon>(root.GetRawText(), options),
                    "Support" => JsonSerializer.Deserialize<SelectRecordSupport>(root.GetRawText(), options),
                    _ => throw new JsonException("JsonConverterSelect.Read() : Not Registered 'Type' property."),
                };
            }
            throw new JsonException("JsonConverterSelect.Read() : Missing or invalid 'Type' property.(NOT Ignore Case)");
        }

        public override void Write(Utf8JsonWriter writer, SelectRecordBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
