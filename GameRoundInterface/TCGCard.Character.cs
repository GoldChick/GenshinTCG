using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCGCard
{
    public enum CharacterType
    {
        Human,
        Mob
    }
    public interface ICardCharacter : ICardBase
    {
        int hp { get; set; }

        CharacterType GetCharacterType();//卡片角色类型
        int GetMaxHP();//最大生命值
        int GetMaxMP();//最大充能
        void GameStartAction();//游戏开始时候的事件
        void OnStageAction();//切换上场的事件
        void OffStageAction();//切换下场的事件
        List<ISkill> GetSkills();//各种技能
    }
}
