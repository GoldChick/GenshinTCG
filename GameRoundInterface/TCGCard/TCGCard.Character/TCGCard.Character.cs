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
        void GameStartAction();//游戏开始时候的事件
        List<ICardSkill> GetSkills();//各种技能
    }
}
