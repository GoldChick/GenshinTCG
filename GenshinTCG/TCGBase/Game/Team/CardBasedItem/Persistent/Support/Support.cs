namespace TCGBase
{
    public class Support : Persistent<AbstractCardPersistentSupport>
    {
        public Support(string nameid) : base(nameid, Registry.Instance.Supports.GetValueOrDefault(nameid))
        {
        }
        public Support(AbstractCardPersistentSupport ef) : base(ef.NameID, ef)
        {
        }
    }
}
