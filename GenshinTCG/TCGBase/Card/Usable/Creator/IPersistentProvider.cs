namespace TCGBase
{
    /// <summary>
    /// 不要实现这个接口
    /// </summary>
    public interface IPersistentProvider
    {
    }
    /// <summary>
    /// 不要实现这个接口
    /// </summary>
    public interface IPersistentProvider<T>:IPersistentProvider where T : AbstractCardPersistent
    {
        /// <summary>
        /// 是否产生在自己队伍身上，默认为true
        /// </summary>
        public virtual bool GenerateOnMe { get => true; }
    }
    /// <summary>
    /// 技能、事件牌、装备牌等实现<br/>
    /// (支援牌已经自动实现)
    /// </summary>
    public interface ISinglePersistentProvider<T> : IPersistentProvider<T> where T : AbstractCardPersistent
    {
        /// <summary>
        /// 可供生成的一种IPersistent,如召唤物:[莫娜:虚影]
        /// </summary>
        public T PersistentPool { get; }
    }
    /// <summary>
    /// 技能、事件牌、装备牌等实现<br/>
    /// (支援牌不允许产生多个persistent)
    /// </summary>
    public interface IMultiPersistentProvider<T> : IPersistentProvider<T> where T : AbstractCardPersistent
    {
        /// <summary>
        /// 可供生成的IPersistent种类,如召唤物:[纯水:蛙+飞鸢+花鼠],出战状态[愚人众伏兵:冰水火雷]
        /// </summary>
        public T[] PersistentPool { get; }
        /// <summary>
        /// 一次从PersistentPool中产生的数量(0-PersistentPool.Length)
        /// </summary>
        public int PersistentNum { get; }
    }
}
