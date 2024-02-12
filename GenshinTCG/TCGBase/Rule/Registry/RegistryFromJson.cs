using System.Text.Json;

namespace TCGBase
{
    public class RegistryFromJson
    {
        public JsonSerializerOptions JsonOptionCharacter { get; }
        public JsonSerializerOptions JsonOptionActionCard { get; }
        public JsonSerializerOptions JsonOptionEffect { get; }
        public JsonSerializerOptions JsonOptionTriggerable { get; }
        public RegistryFromJson()
        {
            JsonOptionCharacter = new()
            {
                Converters = { new JsonConverterTriggerable(), new JsonConverterAction() }
            };
            JsonOptionActionCard = new()
            {
                Converters = { new JsonConverterCondition(), new JsonConverterTriggerable(), new JsonConverterAction() }
            };
            JsonOptionEffect = new()
            {
                Converters = { new JsonConverterTriggerable(), new JsonConverterAction() }
            };
            JsonOptionTriggerable = new()
            {
                Converters = { new JsonConverterTriggerable(), new JsonConverterAction() }
            };
        }
        public void LoadFolders(string path, string modid)
        {
            LoadITriggerable(path, modid, "character", Registry.Instance.CharacterCards.Accept, CreateCharacterCard);
            LoadITriggerable(path, modid, "actioncard", Registry.Instance.ActionCards.Accept, CreateActionCard);
            LoadITriggerable(path, modid, "effect", Registry.Instance.EffectCards.Accept, CreateEffectCard);
        }
        private void LoadITriggerable<T>(string path, string modid, string suffix, Action<T> registry, Func<string, T> create)
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
                foreach (var jsonFile in s)
                {
                    using StreamReader reader = jsonFile.OpenText();
                    var json = reader.ReadToEnd();
                    T t = create(json);
                    if (t is INameSetable namesetable)
                    {
                        namesetable.SetName(modid, jsonFile.Name.Split('.')[0]);
                    }
                    registry(t);
                }
            }
        }
        public CardCharacter CreateCharacterCard(string json)
        {
            CardRecordCharacter? record = JsonSerializer.Deserialize<CardRecordCharacter>(json, JsonOptionCharacter);
            if (record != null)
            {
                return new CardCharacter(record);
            }
            throw new Exception("sb?CardCharacter");
        }
        public AbstractCardAction CreateActionCard(string json)
        {
            CardRecordAction? record = JsonSerializer.Deserialize<CardRecordAction>(json, JsonOptionActionCard);
            if (record != null)
            {
                return record.GetCard();
            }
            throw new Exception("sb?AbstractCardAction");
        }
        public AbstractCardEffect CreateEffectCard(string json)
        {
            CardRecordEffect? record = JsonSerializer.Deserialize<CardRecordEffect>(json, JsonOptionEffect);
            if (record != null)
            {
                return record.GetCard();
            }
            throw new Exception("sb?AbstractCardEffect");
        }
        public AbstractTriggerable CreateTriggerable(string json)
        {
            TriggerableRecordBase? rb = JsonSerializer.Deserialize<TriggerableRecordBase>(json, JsonOptionTriggerable);
            if (rb != null)
            {
                return rb.GetTriggerable();
            }
            throw new Exception("RegistryFromJson:Out Of TriggerableType!");
        }
    }
}
