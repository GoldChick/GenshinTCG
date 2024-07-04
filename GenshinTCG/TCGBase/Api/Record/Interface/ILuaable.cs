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
        public List<object[]?> DoLua(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v, object? @this = null)
        {
            using var lua = LuaManager.CreateLuaEnv(me, p, s, v, @this);
            if (LuaID)
            {
                return Lua.Select(nameid => LuaManager.DoLuaFunction(lua, nameid, me, p, s, v)).ToList();
            }
            else
            {
                try
                {
                    lua.DoString(string.Join('\n', Lua));
                    if (lua["result"] is bool result)
                    {
                        return new() { new object[] { result } };
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"执行lua代码时出现了错误，具体原因: {ex.Message} \n 具体lua代码: {JsonSerializer.Serialize(Lua)}");
                }
                return new() { new object[] { false } };
            }
        }
    }
}
