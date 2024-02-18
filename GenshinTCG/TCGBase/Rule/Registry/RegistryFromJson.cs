using System.Text.Json;

namespace TCGBase
{
    public class RegistryFromJson
    {
        public JsonSerializerOptions JsonOptionGlobal { get; }
        public JsonSerializerOptions JsonOptionCharacter { get; }
        public JsonSerializerOptions JsonOptionActionCard { get; }
        public JsonSerializerOptions JsonOptionEffect { get; }
        public JsonSerializerOptions JsonOptionTriggerable { get; }
        public RegistryFromJson()
        {
            JsonOptionGlobal = new()
            {
                Converters = { new JsonConverterCondition(), new JsonConverterTriggerable(), new JsonConverterAction(), new JsonConverterModifier() }
            };
            JsonOptionCharacter = new()
            {
                Converters = { new JsonConverterCondition(), new JsonConverterTriggerable(), new JsonConverterAction() }
            };
            JsonOptionActionCard = new()
            {
                Converters = { new JsonConverterCondition(), new JsonConverterTriggerable(), new JsonConverterAction() }
            };
            JsonOptionEffect = new()
            {
                Converters = { new JsonConverterCondition(), new JsonConverterTriggerable(), new JsonConverterAction() }
            };
            JsonOptionTriggerable = new()
            {
                Converters = { new JsonConverterCondition(), new JsonConverterTriggerable(), new JsonConverterAction() }
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
                    try
                    {
                        T t = create(json);
                        if (t is INameSetable namesetable)
                        {
                            namesetable.SetName(modid, jsonFile.Name.Split('.')[0]);
                        }
                        registry(t);
                    }
                    catch (Exception ex)
                    {
                        throw new FileLoadException($"在加载文件{jsonFile.Name}时遇到了问题:{ex.Message}");
                    }
                }
            }
        }
        public CardCharacter CreateCharacterCard(string json)
        {
            CardRecordCharacter? record = JsonSerializer.Deserialize<CardRecordCharacter>(json, JsonOptionGlobal);
            if (record != null)
            {
                return new CardCharacter(record);
            }
            throw new Exception("sb?CardCharacter");
        }
        public AbstractCardAction CreateActionCard(string json)
        {
            return JsonSerializer.Deserialize<CardRecordAction>(json, JsonOptionGlobal)?.GetCard() ?? throw new Exception("sb?AbstractCardAction");
        }
        public AbstractCardEffect CreateEffectCard(string json)
        {
            return JsonSerializer.Deserialize<CardRecordEffect>(json, JsonOptionGlobal)?.GetCard() ?? throw new Exception("sb?AbstractCardEffect");
        }
    }
}
