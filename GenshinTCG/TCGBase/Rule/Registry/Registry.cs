using System.Reflection;
namespace TCGBase
{
    internal enum RegistryType
    {
        CharacterCard,
        ActionCard,
    }
    internal class Registry
    {
        private static readonly Registry _instance = new();
        private Registry()
        {
            CharacterCards = new();
            ActionCards = new();
            CardCollections = new RegistryCardCollection[] { CharacterCards, ActionCards, };
        }
        public static Registry Instance { get => _instance; }

        public List<string> Mods { get; } = new();
        internal RegistryCardCollection<AbstractCardCharacter> CharacterCards { get; } = new();
        internal RegistryCardCollection<AbstractCardAction> ActionCards { get; } = new();

        private RegistryCardCollection[] CardCollections { get; }
        //不知为何的namespace黑名单
        private readonly string[] _blacklist = new string[] { "nullable", "null", "blacklist", "minecraft", "equipment", "nilou", "hutao" };

        public bool Contains(RegistryType type, string nameID)
        {
            string[] strs = nameID.Split(':');
            return type switch
            {
                RegistryType.CharacterCard => CharacterCards.ContainsKey(strs[1]),
                RegistryType.ActionCard => ActionCards.ContainsKey(strs[1]),
                _ => false,
            };
        }

        public void Register(AbstractModUtil util)
        {
            string name = util.NameSpace;
            if (Mods.Contains(name) || _blacklist.Contains(name))
            {
                throw new Exception($"Registry:注册名为{name}的mod时出现问题:已经注册过相同namespace的mod!");
            }
            Mods.Add(name);

            AbstractRegister reg = util.GetRegister();

            Array.ForEach(CardCollections, p => p.MoveModToNext(name));

            reg.RegisterCharacter(CharacterCards);
            reg.RegisterActionCard(ActionCards);
        }

        public void LoadDlls(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            DirectoryInfo info = new(path);
            FileInfo[] s = info.GetFiles();
            if (s.Length != 0)
            {
                foreach (var dll in s)
                {
                    if (dll.Name.EndsWith(".dll"))
                    {
                        Assembly ass = Assembly.LoadFile(dll.FullName);

                        Type[] types;
                        try
                        {
                            types = ass.GetTypes();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"加载{dll.Name}时出现了错误！请排查此dll需要的游戏版本等信息！{ex.Message}");
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
    }
}
