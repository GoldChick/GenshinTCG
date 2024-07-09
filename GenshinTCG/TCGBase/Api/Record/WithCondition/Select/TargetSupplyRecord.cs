namespace TCGBase
{
    /// <summary>
    /// 用于Select时，Type只能为Character，Summon，Support，还有Lua
    /// </summary>
    public record class TargetSupplyRecord : ILuaable
    {
        public List<string> Lua { get; }
        public bool LuaID { get; }

        public TargetSupplyRecord(List<string>? lua = null, bool luaID = false)
        {
            Lua = lua ?? new();
            LuaID = luaID;
        }
        public List<Persistent> GetTargets(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
                => (this as ILuaable).DoLua<Persistent>(me, p, s, v);
    }
}
