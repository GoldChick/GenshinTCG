using System.Reflection;
namespace TCGBase
{
    public class RegistryFromDll
    {
        internal List<string> Mods { get; } = new();

        //不知为何的namespace黑名单
        private readonly string[] _blacklist = new string[] { "nullable", "null", "blacklist", "minecraft", "equipment", "nilou", "hutao" };

        public string[] GetMods() => Mods.ToArray();

        internal void Register(AbstractModUtil util)
        {
            string name = util.NameSpace;
            if (Mods.Contains(name) || _blacklist.Contains(name))
            {
                throw new Exception($"Registry:注册名为{name}的mod时出现问题:已经注册过相同namespace的mod!");
            }
            Mods.Add(name);

            AbstractRegister reg = util.GetRegister();

            reg.RegisterTriggerable(Registry.Instance.CustomTriggerable);
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
                            var utils = types.Where(tp => tp.IsSubclassOf(typeof(AbstractModUtil))).Select(tp => Activator.CreateInstance(tp) as AbstractModUtil).ToList();
                            foreach (var util in utils)
                            {
                                if (util != null)
                                {
                                    Register(util);
                                }
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
