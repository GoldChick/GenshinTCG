using System.Reflection;
using TCGCard;
using TCGMod;
using TCGUtil;

namespace TCGRule
{
    internal enum RegistryType
    {
        CharacterCard,
        ActionCard,

        Support,
        Effect,
        Summon
    }
    internal class Registry
    {
        private static Registry _instance;
        private Registry()
        {
            CharacterCards = new();
            ActionCards = new();
            Supports = new();
            Effects = new();
            Summons = new();
            CardCollections = new RegistryCardCollection[] { CharacterCards, ActionCards, Supports, Effects, Summons };
        }
        public static Registry Instance
        {
            get
            {
                _instance ??= new Registry();
                return _instance;
            }
        }

        public List<string> Mods { get; } = new();
        internal RegistryCardCollection<AbstractCardCharacter> CharacterCards { get; } = new();
        internal RegistryCardCollection<AbstractCardAction> ActionCards { get; } = new();

        internal RegistryCardCollection<AbstractCardPersistentSupport> Supports { get; } = new();
        internal RegistryCardCollection<AbstractCardPersistentEffect> Effects { get; } = new();
        internal RegistryCardCollection<AbstractCardPersistentSummon> Summons { get; } = new();

        private RegistryCardCollection[] CardCollections { get; }

        public bool Contains(RegistryType type, string nameID)
        {
            string[] strs = nameID.Split(':');
            return type switch
            {
                RegistryType.CharacterCard => CharacterCards.ContainsKey(strs[1]),
                RegistryType.ActionCard => ActionCards.ContainsKey(strs[1]),
                RegistryType.Support => Supports.ContainsKey(strs[1]),
                RegistryType.Effect => Effects.ContainsKey(strs[1]),
                RegistryType.Summon => Summons.ContainsKey(strs[1]),
                _ => false,
            };
        }

        public void Register(AbstractModUtil util)
        {
            string name = util.NameSpace;
            if (Mods.Contains(name))
            {
                throw new Exception($"Registry:注册名为{name}的mod时出现问题:已经注册过相同namespace的mod!");
            }
            Mods.Add(name);

            AbstractRegister reg = util.GetRegister();

            Array.ForEach(CardCollections, p => p.MoveModToNext(name));

            reg.RegisterCharacter(CharacterCards);
            reg.RegisterActionCard(ActionCards);

            reg.RegisterSupport(Supports);
            reg.RegisterEffect(Effects);
            reg.RegisterSummon(Summons);
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

        /// <summary>
        /// 仅用作测试使用
        /// </summary>
        public void DebugLoad()
        {
            Genshin3_3.Genshin_3_3_Util util = new();
            Register(util);
        }
        public void Print()
        {
            Print("CharacterCards", CharacterCards);
            Print("ActionCards", ActionCards);

            Print("Supports", Supports);
            Print("Effects", Effects);
            Print("Summons", Summons);
        }
        private static void Print<T>(string name, RegistryCardCollection<T> dic) where T : AbstractCardBase
        {
            Logger.Print($"{name}:");
            dic.Print();
        }
    }
}
