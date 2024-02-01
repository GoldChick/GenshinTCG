namespace TCGBase
{
    public abstract class AbstractCardBase
    {
        public string Namespace => (GetType().Namespace ?? "minecraft").ToLower();
        public virtual string NameID { get => GetType().Name.ToLower(); }
        public List<string> Tags { get; }
        protected AbstractCardBase()
        {
            Tags = new();
        }
        internal AbstractCardBase(BaseCardRecord record)
        {
            Tags = record.Tags;
        }
    }
}
