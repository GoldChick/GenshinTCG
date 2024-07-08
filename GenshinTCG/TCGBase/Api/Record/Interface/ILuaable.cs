using NLua;
using System.Collections;
using System.Text.Json;

namespace TCGBase
{
    internal interface ILuaable
    {
        public List<string> Lua { get; }
        /// <summary>
        /// 默认为false<br/>
        /// 为true时代表Lua中的string都是"minecraft:enchant.cryo"这种id<br/>
        /// 为false时就是直接的代码，直接运行，名叫result的变量替代作为返回值
        /// </summary>
        public bool LuaID { get; }
        public void DoLua(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v, object? @this = null)
        {
            using var lua = LuaManager.CreateLuaEnv(me, p, s, v, @this);
            if (LuaID)
            {
                Lua.ForEach(nameid => LuaManager.DoLuaFunction(lua, nameid, me, p, s, v));
            }
            else
            {
                try
                {
                    lua.DoString(string.Join('\n', Lua));
                }
                catch (Exception ex)
                {
                    throw new Exception($"执行lua代码时出现了错误，具体原因: {ex.Message} \n 具体lua代码: {JsonSerializer.Serialize(Lua)}");
                }
            }
        }
        public List<T> DoLua<T>(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v, object? @this = null)
        {
            using var lua = LuaManager.CreateLuaEnv(me, p, s, v, @this);
            List<T> results = new();

            if (LuaID)
            {
                foreach (var objects in Lua.Select(nameid => LuaManager.DoLuaFunction(lua, nameid, me, p, s, v)).ToList())
                {
                    foreach (var item in objects)
                    {
                        if (item is T t)
                        {
                            results.Add(t);
                        }
                    }
                }
            }
            else
            {
                try
                {
                    lua.DoString(string.Join('\n', Lua));
                    if (lua["result"] is LuaTable table)
                    {
                        foreach (var item in table.Values)
                        {
                            if (item is T t)
                            {
                                results.Add(t);
                            }
                        }
                    }
                    else if (lua["result"] is T t)
                    {
                        results.Add(t);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"执行lua代码并且想要获得{nameof(T)}结果时出现了错误，具体原因: {ex.Message} \n 具体lua代码: {JsonSerializer.Serialize(Lua)}");
                }
            }
            return results;
        }
        /// <summary>
        /// (所有的)lua脚本都返回true
        /// </summary>
        public bool Valid(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v, object? @this = null)
        {
            var bools = DoLua<bool>(me, p, s, v, @this);
            return bools.Any() && bools.All(p => p);
        }
    }
}
