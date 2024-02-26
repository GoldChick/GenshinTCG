namespace TCGBase
{
    public abstract class AbstractPersistentSet
    {
        public int PersistentRegion { get; init; }
        protected readonly PlayerTeam _me;
        protected private AbstractPersistentSet(PlayerTeam me)
        {
            _me = me;
        }
    }
}
