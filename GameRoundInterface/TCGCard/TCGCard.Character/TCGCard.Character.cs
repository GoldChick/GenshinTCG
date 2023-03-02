using System;
using System.Collections.Generic;

namespace TCGCard
{
    public interface ICardCharacter : ICardBase
    {
        public int MaxHP { get; }
        public int MaxMP { get; }
        /// <summary>
        /// Nullable
        /// </summary>
        public ICardEffect DefaultEffect { get; }
        public List<ICardSkill> Skills { get; }
        /// <summary>
        /// 默认携带的属性
        /// </summary>
        public Dictionary<string, Enum> Attributes { get; }
    }
}
