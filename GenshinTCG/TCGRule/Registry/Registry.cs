using System.Reflection;
using System.Text.Json;
using TCGCard;
using TCGMod;
using TCGUtil;

namespace TCGRule
{
    public class Consumer<T> where T : ICardBase
    {
        private readonly Dictionary<string, T> _values;

        public Consumer(Dictionary<string, T> values)
        {
            _values = values;
        }

        public void Accept(T t) => Logger.Error($"Registry:注册名为{t.Name}的{typeof(T)}时出现了问题!", !_values.TryAdd(t.Name, t));
    }

    internal class Registry
    {
        private static Registry _instance;
        public static Registry Instance
        {
            get
            {
                _instance??= new Registry();
                return _instance;
            }
        }

        public List<string> Mods { get; } = new();
        public Dictionary<string, Dictionary<string, ICardCharacter>> CharacterCards { get; } = new();
        public Dictionary<string, Dictionary<string, ICardAction>> ActionCards { get; } = new();

        public Dictionary<string, Dictionary<string, ISupport>> Supports { get; } = new();
        public Dictionary<string, Dictionary<string, IEffect>> Effects { get; } = new();
        public Dictionary<string, Dictionary<string, ISummon>> Summons { get; } = new();

        public void Register(AbstractModUtil util)
        {
            string name = util.NameSpace;
            if (Mods.Contains(name))
            {
                throw new Exception($"Registry:注册名为{name}的mod时出现问题:已经注册过相同namespace的mod!");
            }
            Mods.Add(name);

            CharacterCards.Add(name, new());
            ActionCards.Add(name, new());

            Supports.Add(name, new());
            Effects.Add(name, new());
            Summons.Add(name, new());

            AbstractRegister reg = util.GetRegister();

            reg.RegisterCharacter(new(CharacterCards[name]));
            reg.RegisterActionCard(new(ActionCards[name]));

            reg.RegisterSupport(new(Supports[name]));
            reg.RegisterEffect(new(Effects[name]));
            reg.RegisterSummon(new(Summons[name]));

        }

        public void LoadDlls(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Logger.Print("创建了 \"mods\"文件夹！");
            }
            DirectoryInfo info = new(path);
            FileInfo[] s = info.GetFiles();
            if (s.Length == 0)
            {
                Logger.Warning("不存在可以读取的Mod！请尝试下载一些，否则无法正常运行游戏！");
            }
            else
            {
                foreach (var dll in s)
                {
                    if (dll.Name.EndsWith(".dll"))
                    {
                        Assembly ass = Assembly.LoadFile(dll.FullName);
                        Logger.Print(ass.FullName);

                        Type[] types;
                        try
                        {
                            types = ass.GetTypes();
                        }
                        catch (Exception)
                        {
                            throw new Exception($"加载{dll.Name}时出现了错误！请排查此dll需要的游戏版本等信息！");
                        }

                        try
                        {
                            Type? tp = Array.Find(types, tp => tp.IsSubclassOf(typeof(AbstractModUtil)));
                            if (tp != null && Activator.CreateInstance(tp) is AbstractModUtil util)
                            {
                                Register(util);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"加载名为{dll.Name}的文件时出现了错误！" +
                                $"没有发现实现AbstractModUtil的类或者该类存在一些问题！详细信息：{ex.Message}");
                        }
                    }
                }
            }
        }
        public void Print()
        {
            Logger.Print(JsonSerializer.Serialize(Mods));

            Logger.Print(JsonSerializer.Serialize(CharacterCards));
            Logger.Print(JsonSerializer.Serialize(ActionCards));

            Logger.Print(JsonSerializer.Serialize(Supports));
            Logger.Print(JsonSerializer.Serialize(Effects));
            Logger.Print(JsonSerializer.Serialize(Summons));
        }
    }
}
