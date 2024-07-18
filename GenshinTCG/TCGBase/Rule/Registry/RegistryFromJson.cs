using System.Text.Json;

namespace TCGBase
{
    public class RegistryFromJson
    {
        public readonly static JsonSerializerOptions JsonOptionGlobal = new()
        {
            Converters = { new JsonConverterCondition(), new JsonConverterTriggerable(), new JsonConverterAction(), new JsonConverterModifier() }
        };
        public RegistryFromJson()
        {
        }
        public void LoadFolders(string path, string modid)
        {
            LoadITriggerable(path, modid, "character", Registry.Instance.CharacterCards.Accept, CreateCharacterCard);
            LoadITriggerable(path, modid, "actioncard", Registry.Instance.ActionCards.Accept, CreateActionCard);
            LoadITriggerable(path, modid, "effect", Registry.Instance.EffectCards.Accept, CreateEffectCard);
            LoadITriggerable(path, modid, "listener", Registry.Instance.ListenerCards.Accept, CreateListenerCard);
            LoadLuaScripts(path, modid);
        }
        private static void LoadITriggerable<T>(string path, string modid, string suffix, Action<T> registry, Func<string, T> create)
        {
            LoadFiles(path, modid, suffix, (file) =>
            {
                using StreamReader reader = file.OpenText();
                var json = reader.ReadToEnd();
                T t = create(json);
                if (t is INameSetable namesetable)
                {
                    namesetable.SetName(modid, file.Name.Split('.')[0]);
                }
                registry(t);
            });
        }
        private static void LoadLuaScripts(string path, string modid)
        {
            LoadFiles(path, modid, "lua", (luaFile) =>
            {
                using StreamReader reader = luaFile.OpenText();
                var luascript = reader.ReadToEnd();
                Registry.Instance.LuaScripts.Add($"{modid}:{luaFile.Name.Split('.')[0]}", luascript);
            });
        }
        private static void LoadFiles(string path, string modid, string suffix, Action<FileInfo> action2filecontent)
        {
            path = $"{path}/{modid}/{suffix}";
            if (!Directory.Exists(path))
            {
                return;
            }
            DirectoryInfo info = new(path);
            FileInfo[] s = info.GetFiles();
            if (s.Any())
            {
                foreach (var file in s)
                {
                    try
                    {
                        action2filecontent(file);
                    }
                    catch (Exception ex)
                    {
                        throw new FileLoadException($"在加载文件{file.Name}时遇到了问题:{ex.Message}");
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
        public CardEffect CreateEffectCard(string json)
        {
            return JsonSerializer.Deserialize<CardRecordEffect>(json, JsonOptionGlobal)?.GetCard() ?? throw new Exception("sb?AbstractCardEffect");
        }
        public CardListener  CreateListenerCard(string json)
        {
            return JsonSerializer.Deserialize<CardRecordListener>(json, JsonOptionGlobal)?.GetCard() ?? throw new Exception("sb?AbstractCardListener");
        }
    }
}
