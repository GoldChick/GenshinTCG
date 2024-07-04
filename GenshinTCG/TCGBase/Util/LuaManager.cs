using NLua;

namespace TCGBase
{
    internal static class LuaManager
    {
        public static Lua CreateLuaEnv(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v, object? @this = null)
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
        /// nameid: 格式如"minecraft:modifier_enchant.Cryo"
        /// </summary>
        public static object[]? DoLuaFunction(Lua lua, string nameid, params object?[] args)
        {
            try
            {
                var array = nameid.Split('.');
                if (Registry.Instance.LuaScripts.TryGetValue(array[0], out var script))
                {
                    lua.DoString(script);
                    return (lua[array.ElementAtOrDefault(1) ?? "Main"] as LuaFunction)?.Call(args);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"执行lua脚本{nameid}出现问题，详细原因：{e.Message}");
            }
            return null;
        }
    }
}
