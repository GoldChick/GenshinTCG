namespace TCGBase
{
    public abstract class AbstractCardBase
    {
        public string Namespace => (GetType().Namespace ?? "minecraft").ToLower();
        public virtual string NameID { get => GetType().Name.ToLower(); }
    }
}
