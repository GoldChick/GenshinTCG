namespace TCGGame
{
    /// <summary>
    /// TODO:将来可以修改Effect触发的机制，用空间换时间
    /// </summary>
    internal class PersistentDictionary
    {
        private readonly Dictionary<string, AbstractPersistent> _data;

        public PersistentDictionary()
        {
            _data = new();
        }
        public void Add(AbstractPersistent per)
        {

        }
    }
}
