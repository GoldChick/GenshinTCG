using System.IO;
using System.Reflection;
using System.Text.Json;

namespace TCGBase
{
    public class RegistryFromJson
    {
        public JsonSerializerOptions JsonOptionActionCard { get; }
        public JsonSerializerOptions JsonOptionTriggerable { get; }
        public RegistryFromJson()
        {
            JsonOptionActionCard = new()
            {
                Converters = { new JsonConverterActionCard(), new JsonConverterSelect(), new JsonConverterCondition() }
            };
            JsonOptionTriggerable = new()
            {
                Converters = { new JsonConverterTriggerable() }
            };
        }
        public void LoadFolders(string path, string modid)
        {
            LoadITriggerable(path, modid, "character", (json) => Registry.Instance.CharacterCards.Accept(CreateCharacterCard(json, modid)));
            LoadITriggerable(path, modid, "actioncard", (json) => Registry.Instance.ActionCards.Accept(CreateActionCard(json, modid)));
            LoadITriggerable(path, modid, "effect", (json) => Registry.Instance.EffectCards.Accept(CreateEffectCard(json, modid)));
        }
        private void LoadITriggerable(string path, string modid, string suffix, Action<string> registry)
        {
            path = $"{path}/{modid}/{suffix}";
            if (!Directory.Exists(path))
            {
                return;
            }
            DirectoryInfo info = new(path);
            var s = info.GetFiles();
            if (s.Length != 0)
            {
                foreach (var jsonF in s)
                {
                    using StreamReader reader = jsonF.OpenText();
                    var json = reader.ReadToEnd();
                    registry(json);
                }
            }
        }
        public AbstractCardCharacter CreateCharacterCard(string json, string modid)
        {
            try
            {
                CardRecordCharacter? record = JsonSerializer.Deserialize<CardRecordCharacter>(json);
                if (record != null)
                {
                    return new CardCharacter(record, modid);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            throw new Exception("sb?");
        }
        public AbstractCardAction CreateActionCard(string json, string modid)
        {
            try
            {
                CardRecordAction? record = JsonSerializer.Deserialize<CardRecordAction>(json, JsonOptionActionCard);
                if (record != null)
                {
                    return record.GetCard(modid);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            throw new Exception("sb?");
        }
        public AbstractCardEffect CreateEffectCard(string json, string modid)
        {
            try
            {
                CardRecordEffect? record = JsonSerializer.Deserialize<CardRecordEffect>(json);
                if (record != null)
                {
                    return record.GetCard(modid);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            throw new Exception("sb?");
        }

        public ITriggerable CreateTriggerable(string json)
        {
            try
            {
                TriggerableRecordBase? rb = JsonSerializer.Deserialize<TriggerableRecordBase>(json, JsonOptionTriggerable);
                if (rb != null)
                {
                    return rb.GetTriggerable();
                }
            }
            catch (Exception)
            {
                throw;
            }
            throw new Exception("RegistryFromJson:Out Of TriggerableType!");
        }
    }
}
