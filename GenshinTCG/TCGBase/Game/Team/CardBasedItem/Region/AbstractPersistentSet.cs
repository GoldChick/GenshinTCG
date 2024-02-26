namespace TCGBase
{
    public abstract class AbstractPersistentSet
    {
        public int PersistentRegion { get; init; }
    }
    public abstract class AbstractPersistentSet<T> : AbstractPersistentSet where T : AbstractCardBase
    {
        protected readonly List<Persistent<T>> _data;
        protected private AbstractPersistentSet()
        {
            _data = new();
        }
    }
}
