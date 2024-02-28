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
                    //↓下为不那么具体的具体类预设↓
                    "Custom" => JsonSerializer.Deserialize<TriggerableRecordCustom>(root.GetRawText(), options),
                    "Skill" => JsonSerializer.Deserialize<TriggerableRecordSkill>(root.GetRawText(), options),
                    "Prepare" => JsonSerializer.Deserialize<TriggerableRecordPrepare>(root.GetRawText(), options),
                    "AfterUseSkill" or "AfterUseCard" => JsonSerializer.Deserialize<TriggerableRecordEnable>(root.GetRawText(), options),
                    //↓下为具体类预设↓
                    "Skill_A" or "Skill_E" => JsonSerializer.Deserialize<TriggerableRecordSkillPreset>(root.GetRawText(), options),
                    _ => JsonSerializer.Deserialize<TriggerableRecordBaseImplement>(root.GetRawText(), options),
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
