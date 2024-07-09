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
                if (Enum.TryParse(typeElement.GetString(), out TriggerType type))
                {
                    return type switch
                    {
                        TriggerType.Element=> JsonSerializer.Deserialize<ActionRecordString>(root.GetRawText(), options),
                        TriggerType.Damage => JsonSerializer.Deserialize<ActionRecordDamage>(root.GetRawText(), options),
                        TriggerType.EatDice => JsonSerializer.Deserialize<ActionRecordEatDice>(root.GetRawText(), options),
                        TriggerType.SampleEffect => JsonSerializer.Deserialize<ActionRecordSampleEffect>(root.GetRawText(), options),
                        TriggerType.DrawCard => JsonSerializer.Deserialize<ActionRecordDrawCard>(root.GetRawText(), options),
                        TriggerType.Switch or TriggerType.Destroy => JsonSerializer.Deserialize<ActionRecordBaseWithTarget>(root.GetRawText(), options),

                        _ => JsonSerializer.Deserialize<ActionRecordLua>(root.GetRawText(), options),
                    };
                }
            }
            return JsonSerializer.Deserialize<ActionRecordLua>(root.GetRawText(), options);
        }

        public override void Write(Utf8JsonWriter writer, ActionRecordBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
