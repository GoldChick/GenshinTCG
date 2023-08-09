namespace TCGCard
{
    public interface ICardCharacter : ICardServer
    {
        /// <summary>
        /// 主元素，用于调和和携带共鸣牌的判定等
        /// </summary>
        public string MainElement { get; }
        public int MaxHP { get; }
        public int MaxMP { get; }
        /// <summary>
        /// @Nullable
        /// </summary>
        public IEffect? DefaultEffect { get; }
        /// <summary>
        ///  @NonNull
        /// </summary>
        public ICardSkill[] Skills { get; }
    }
}
