using System;
using System.Collections.Generic;

namespace TCGCard
{
    public enum CharacterType
    {
        Human,
        Mob
    }
    public enum CharacterRegion
    {
        Abyss,
        Mondstadt,
        Liyue,
        Inazuma,
        Sumeru,
        Fontaine,
        Natlan
    }
    public interface ICardCharacter : ICardBase
    {
        public CharacterType CharacterType { get; }
        public CharacterRegion CharacterRegion { get; }
        public int MaxHP { get; }
        public int MaxMP { get; }
        /// <summary>
        /// Nullable
        /// </summary>
        public ICardEffect DefaultEffect { get; }
        public List<ICardSkill> Skills { get; }
    }
}
