using NLua;

namespace TCGBase
{
    internal static class LuaManager
    {
        private static Lua? _lua;
        private static Lua Lua
        {
            get
            {
                if (_lua == null)
                {
                    _lua = new Lua();
                    _lua.LoadCLRPackage();
                    _lua.DoString("import('GenshinTCG,'TCGBase')");
                    _lua.DoString("import('System.Linq')");
                    RegisterDelegate();
                }
                return _lua;
            }
        }
        private static void RegisterDelegate()
        {
            Lua.RegisterLuaDelegateType(typeof(Action), typeof(LuaActionHandler));
            Lua.RegisterLuaDelegateType(typeof(Action<int>), typeof(LuaActionHandler<int>));
            Lua.RegisterLuaDelegateType(typeof(Predicate<int>), typeof(LuaPredicateHandler<int>));
        }
        public static Lua CreateLuaEnv(PlayerTeam me, Persistent p, SimpleSender s, AbstractVariable? v, object? @this = null)
        {
            Lua["this"] = @this;
            Lua["me"] = me;
            Lua["p"] = p;
            Lua["s"] = s;
            Lua["v"] = v;
            return Lua;
        }
        /// <summary>
        /// nameid_params: <br/>
        /// 格式如"minecraft:modifier_enchant.Cryo"<br/>
        /// 可以加字符串参数如"minecraft:action_persistent.AddSummon+minecraft:summon_fischl" (实际没有这个方法)
        /// </summary>
        public static object[] DoLuaFunction(Lua lua, string nameid_params, params object?[] args)
        {
            try
            {
                var array_origin = nameid_params.Split('+');
                if (array_origin.Length >= 1)
                {
                    var array_nameid = array_origin[0].Split('.');
                    if (Registry.Instance.LuaScripts.TryGetValue(array_nameid[0], out var script))
                    {
                        lua.DoString(script);

                        object?[] final_args = args.Concat(array_origin[1..]).ToArray();

                        return (lua[array_nameid.ElementAtOrDefault(1) ?? "Main"] as LuaFunction)?.Call(final_args) ?? Array.Empty<object[]>();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"执行lua脚本{nameid_params}出现问题，详细原因：{e.Message}");
            }
            return Array.Empty<object[]>();
        }
    }
}
