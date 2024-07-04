using System.Text.Json;
using System.Text.Json.Serialization;

namespace TCGBase
{
    public class JsonConverterModifier : JsonConverter<ModifierRecordBase>
    {
        public override ModifierRecordBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            if (root.TryGetProperty("Type", out JsonElement typeElement))
            {
                if (Enum.TryParse(typeElement.GetString(), out ModifierType type))
                {
                    return type switch
                    {
                        ModifierType.Dice => JsonSerializer.Deserialize<ModifierRecordDice>(root.GetRawText(), options),
                        ModifierType.Fast => JsonSerializer.Deserialize<ModifierRecordFast>(root.GetRawText(), options),
                        _ => JsonSerializer.Deserialize<ModifierRecordBaseImpl>(root.GetRawText(), options),
                    };
                }
            }
            return JsonSerializer.Deserialize<ModifierRecordBaseImpl>(root.GetRawText(), options);
        }

        public override void Write(Utf8JsonWriter writer, ModifierRecordBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
