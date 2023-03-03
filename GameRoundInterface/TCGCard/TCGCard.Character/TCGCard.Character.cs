namespace TCGCard
{
    public interface ICardCharacter : ICardServer
    {
        public int MaxHP { get; }
        public int MaxMP { get; }
        /// <summary>
        /// Nullable
        /// </summary>
        public ICardEffect DefaultEffect { get; }
        public ICardSkill[] Skills { get; }
    }
}
