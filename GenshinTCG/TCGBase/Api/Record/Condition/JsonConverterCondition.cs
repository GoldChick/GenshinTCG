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
                        ConditionType.Counter=> JsonSerializer.Deserialize<ConditionRecordTriInt>(root.GetRawText(), options),

                        ConditionType.HasEffect or ConditionType.HasEffectWithTag  or ConditionType.HasCard or ConditionType.OperationType
                        or ConditionType.SimpleTalent or ConditionType.OurCharacterCause or ConditionType.ThisCharacterCause or ConditionType.SimpleWeapon
                        or ConditionType.ElementRelated
                        => JsonSerializer.Deserialize<ConditionRecordString>(root.GetRawText(), options),

                        ConditionType.AnyTarget or ConditionType.AnyTargetWithSameIndex or ConditionType.CanBeAppliedFrom
                        => JsonSerializer.Deserialize<ConditionRecordTarget>(root.GetRawText(), options),

                        ConditionType.Compound
                        => JsonSerializer.Deserialize<ConditionRecordCompound>(root.GetRawText(), options),

                        _ => JsonSerializer.Deserialize<ConditionRecordLua>(root.GetRawText(), options),
                    };
                }
            }
            return JsonSerializer.Deserialize<ConditionRecordLua>(root.GetRawText(), options);
        }

        public override void Write(Utf8JsonWriter writer, ConditionRecordBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
