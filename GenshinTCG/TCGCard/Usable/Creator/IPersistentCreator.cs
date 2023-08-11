namespace TCGCard
{
    public interface IPersistentCreator
    {
    }
    public interface IPersistentCreator<T>:IPersistentCreator where T: IPersistent
    {
        /// <summary>
        /// 可供生成的IPersistent种类,如召唤物:[纯水:蛙+飞鸢+花鼠],[菲谢尔:奥兹]
        /// </summary>
        public T[] PersistentPool { get; }
        /// <summary>
        /// 为True则按照顺序生成,为False则随机生成
        /// </summary>
        public bool PersistentOrdered { get; }
        /// <summary>
        /// 一次从PersistentPool中产生的数量
        /// </summary>
        public int PersistentNum { get; }
    }
    public interface IAssistCreator : IPersistentCreator<ISupport>
    {
    }
    public interface IEffectCreator : IPersistentCreator<IEffect>
    {
    }
    public interface ISummonCreator : IPersistentCreator<ISummon>
    {
    }
    
}
