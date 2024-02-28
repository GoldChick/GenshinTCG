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
                        ModifierType.Damage => JsonSerializer.Deserialize<ModifierRecordDamage>(root.GetRawText(), options),
                        ModifierType.Dice => JsonSerializer.Deserialize<ModifierRecordDice>(root.GetRawText(), options),
                        ModifierType.Fast => JsonSerializer.Deserialize<ModifierRecordFast>(root.GetRawText(), options),
                        ModifierType.Enchant => JsonSerializer.Deserialize<ModifierRecordEnchant>(root.GetRawText(), options),
                        _ => JsonSerializer.Deserialize<ModifierRecordBaseImplement>(root.GetRawText(), options),
                    };
                }
            }
            throw new JsonException($"JsonConverterModifier.Read() : Missing or invalid 'Type' property:(NOT Ignore Case)Json: \n{root}");
        }

        public override void Write(Utf8JsonWriter writer, ModifierRecordBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
