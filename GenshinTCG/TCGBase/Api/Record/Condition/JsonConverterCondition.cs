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
                if (Enum.TryParse(typeElement.GetString(), out ConditionType type))
                {
                    return type switch
                    {
                        ConditionType.HPLost or ConditionType.Counter or ConditionType.Damage
                        => JsonSerializer.Deserialize<ConditionRecordTriInt>(root.GetRawText(), options),

                        ConditionType.HasEffect or ConditionType.HasEffectWithTag or ConditionType.HasTag or ConditionType.Name
                        or ConditionType.Element or ConditionType.Reaction or ConditionType.SkillType or ConditionType.Related
                        => JsonSerializer.Deserialize<ConditionRecordString>(root.GetRawText(), options),

                        _ => JsonSerializer.Deserialize<ConditionRecordBaseImplement>(root.GetRawText(), options),
                    };
                }
                throw new JsonException($"Unregistered Condition 'Type' property: {typeElement.GetString()}.");
            }
            throw new JsonException("Missing or invalid 'Type' property In Condition Record.(NOT Ignore Case)");
        }

        public override void Write(Utf8JsonWriter writer, ConditionRecordBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
