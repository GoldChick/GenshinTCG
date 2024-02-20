namespace TCGBase
{
    public interface IWhenThenAction
    {
        public List<ConditionRecordBase> When { get; }
        public bool IsConditionValid(PlayerTeam me, Persistent p, AbstractSender s, AbstractVariable? v)
        {
            return When.TrueForAll(condition => condition.Valid(me, p, s, v));
        }
    }
}
