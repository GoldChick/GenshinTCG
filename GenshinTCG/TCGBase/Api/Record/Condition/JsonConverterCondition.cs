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
                        ConditionType.Lua => JsonSerializer.Deserialize<ConditionRecordLua>(root.GetRawText(), options),

                        ConditionType.HPLost or ConditionType.MPLost or ConditionType.Counter or ConditionType.Damage
                        or ConditionType.HP or ConditionType.MP or ConditionType.DataCount or ConditionType.DataContains or ConditionType.Region 
                       
                        => JsonSerializer.Deserialize<ConditionRecordTriInt>(root.GetRawText(), options),

                        ConditionType.HasEffect or ConditionType.HasEffectWithTag or ConditionType.HasTag or ConditionType.HasCard or ConditionType.Name or ConditionType.OperationType
                        or ConditionType.SimpleTalent or ConditionType.OurCharacterCause or ConditionType.ThisCharacterCause or ConditionType.SimpleWeapon
                        or ConditionType.Element or ConditionType.ElementReaction or ConditionType.SkillType or ConditionType.ElementRelated
                        => JsonSerializer.Deserialize<ConditionRecordString>(root.GetRawText(), options),

                        ConditionType.AnyTarget or ConditionType.AnyTargetWithSameIndex or ConditionType.CanBeAppliedFrom
                        => JsonSerializer.Deserialize<ConditionRecordTarget>(root.GetRawText(), options),

                        ConditionType.Compound
                        => JsonSerializer.Deserialize<ConditionRecordCompound>(root.GetRawText(), options),

                        _ => JsonSerializer.Deserialize<ConditionRecordBase>(root.GetRawText(), options),
                    };
                }
                throw new JsonException($"Unregistered Condition 'Type' property: {typeElement.GetString()}.\n detail: \n{root}");
            }
            throw new JsonException($"Missing or invalid 'Type' property In Condition Record.(NOT Ignore Case). json:\n {root}");
        }

        public override void Write(Utf8JsonWriter writer, ConditionRecordBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
