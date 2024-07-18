using NLua;

namespace TCGBase
{
    internal static class LuaManager
    {
        public static Lua CreateLuaEnv(PlayerTeam me, Persistent p, SimpleSender s, AbstractVariable? v, object? @this = null)
        {
            Lua lua = new();
            lua.LoadCLRPackage();
            lua.DoString("import('GenshinTCG,'TCGBase')");
            lua.DoString("import('System.Linq')");
            lua["this"] = @this;
            lua["me"] = me;
            lua["p"] = p;
            lua["s"] = s;
            lua["v"] = v;
            return lua;
        }
        /// <summary>
        /// nameid_params: <br/>
        /// 格式如"minecraft:modifier_enchant.Cryo"<br/>
        /// 可以加字符串参数如"minecraft:action_persistent.AddSummon+minecraft:summon_fischl" (实际没有这个方法)
        /// </summary>
        public static object[] DoLuaFunction(Lua lua, string nameid_params, params object[] args)
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

                        object[] final_args = args.Concat(array_origin[1..]).ToArray();

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
