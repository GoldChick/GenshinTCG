namespace TCGCard
{
    public abstract class AbstractCardCharacter : AbstractCardServer
    {
        public virtual int MaxHP { get => 10; }
        public virtual int MaxMP { get => 2; }
        /// <summary>
        /// 主元素，用于调和和携带共鸣牌的判定等
        /// </summary>
        public abstract string MainElement { get; }
        /// <summary>
        /// @Nullable
        /// TODO: check it?
        /// </summary>
        public virtual AbstractCardPersistentEffect? DefaultEffect { get=>null; }
        /// <summary>
        ///  @NonNull
        /// </summary>
        public abstract AbstractCardSkill[] Skills { get; }

    }
}
