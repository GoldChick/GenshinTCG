namespace TCGBase
{
    /// <summary>
    /// 提供给targetrecord使用
    /// </summary>
    internal interface IPeristentSupplier
    {
        public Persistent Persistent { get; }
    }
    internal interface IPersistentIndirectSupplier
    {
        public Persistent GetPersistent(PlayerTeam team);
    }
    /// <summary>
    /// 提供给targetrecord使用
    /// </summary>
    internal interface IMulPersistentSupplier
    {
        public IEnumerable<Persistent> Persistents { get; }
    }
    internal interface IMulPersistentIndirectSupplier
    {
        public IEnumerable<Persistent> GetPersistent(PlayerTeam team);
    }
}
