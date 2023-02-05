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
        CharacterType GetCharacterType();
        CharacterRegion GetCharacterRegion();
        int GetMaxHP();//最大生命值
        int GetMaxMP();//最大充能
        void GameStartAction();//游戏开始时候的事件
        List<ISkill> GetSkills();//各种技能
    }
}
