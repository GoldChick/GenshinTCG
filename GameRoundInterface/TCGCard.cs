using System.Collections.Generic;
using System.Drawing;
using TCGGame;
//################################################################
//额外制作的卡牌dll实现这些接口
//在Unity中调用查询信息
//################################################################
namespace TCGCard
{
    /// <summary>
    /// 卡片类型
    ///总体分为[角色卡]和[辅助卡]
    ///<para></para>
    ///[辅助卡]包括[天赋卡][场地卡][武器卡][圣遗物卡][食物卡][事件卡][召唤物卡]等
    /// </summary>
    public enum CardType
    {
        Character,

        Nature,
        Weapon,
        Artifact,
        Place,
        Food,
        Event,
        Summon
    }
    public enum WeaponType
    {
        Other,
        Sword,
        BigSword,
        LongWeapon,
        Catalyst,
        Bow
    }

    public interface ICardBase
    {
        string GetCardName();
        CardType GetCardType();
        Bitmap GetImageBmp();
    }

    namespace CardInterface
    {
        public interface ICardAssist : ICardBase
        {
            int GetUseTimes();//能够使用的最大次数
            bool CanBeMultiplyUsed();//是否能够多次使用，即每回合开始阶段刷新。否则次数耗尽后消失
            string GetCardTexts();//卡牌说明
            int GetDiceNum();//骰子数量 
            bool IsSameDice();//是否需要相同的骰子
            bool CanBeArmed();//是否可以加入卡组里
            bool CanBeUsed(IGameBase[] igame);//是否满足打出条件
            bool CanBeActived(IGameBase[] igame);//（打出后的牌）是否满足生效的条件
            void AfterUseAction(IGameBase[] igame);//使用后立即发生的事件
        }

        public interface ICardNature : ICardAssist
        {
            ICardCharacter GetCharacter();//所属的角色
            ISkill GetSkill();//所强化的技能
            int GetAdditionalDamage(IGameBase[] igame);//一定条件下额外攻击
            void GetAdditonalEffect(IGameBase[] igame);//一定条件下触发的额外效果
        }
        public interface ICardWeapon : ICardAssist
        {
            WeaponType GetWeaponType();//武器的类型
            int GetBaseDamage();//武器基础攻击
            int GetAdditionalDamage(IGameBase[] igame);//一定条件下额外攻击
            void GetAdditonalEffect(IGameBase[] igame);//一定条件下触发的额外效果
        }
        

    }
}



