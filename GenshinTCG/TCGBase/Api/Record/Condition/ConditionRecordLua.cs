namespace TCGBase
{
    public record class ConditionRecordLua : ConditionRecordBase, ILuaable
    {
        public List<string> Lua { get; }
        public bool LuaID { get; }
        public ConditionRecordLua(List<string>? lua = null, bool luaID = false, bool not = false, ConditionRecordBase? or = null) : base(ConditionType.Lua, not, or)
        {
            Lua = lua ?? new();
            LuaID = luaID;
        }
        protected override bool GetPredicate(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            return (this as ILuaable).Valid(me,p,s,v);
        }
    }
}