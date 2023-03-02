using System;
using System.Collections.Generic;

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
        public List<ICardSkill> Skills { get; }

    }
}
