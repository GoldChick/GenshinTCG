using System.Collections.Generic;
using System.Drawing;
using TCGCard;
using TCGCard.CardInterface;
using TCGCard.Skill;
using TCGGame;
using TCGInfo.InfoInterface;
//################################################################
//额外制作的卡牌dll实现这些接口
//在Unity中调用查询信息
//################################################################
namespace TCGCard
{
    //################################################################
    //元素类型(实际上并不仅仅是元素)
    // 0为Trival，对应物理攻击/万能元素骰
    // 1-7为七元素
    //################################################################
    public enum ElementType
    {
        Trival,
        Pyro,//火
        Hydro,//水
        Anemo,//风
        Electro,//雷
        Dendro,//草
        Cryo,//冰
        Geo//岩
    }
    //################################################################
    //卡片类型
    //总体分为[角色卡]和[辅助卡]
    //[辅助卡]包括[天赋卡][场地卡][武器卡][圣遗物卡][食物卡][事件卡][召唤物卡]等
    //################################################################
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
    //################################################################
    //[角色卡]的种类，目前有[人类]和[魔物]两种
    //################################################################
    public enum CharacterType
    {
        Human,
        Mob
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
    namespace Skill
    {
        public enum SkillType
        {
            Passive,
            NormalAttack,
            E,
            Q
        }
        public interface ISkill
        {
            string GetSkillName();
            string GetSkillText();
            bool IsSameDice(); //sameDice控制costs中的Trival元素是否需要相同
            SkillType GetSkillType();
            List<ElementType> GetCosts();//同时兼顾数量与种类
            Bitmap GetImageBmp();
            void OnUseAction();
        }
    }
    namespace CardInterface
    {
        public interface ICardBase
        {
            string GetCardName();
            CardType GetCardType();
            Bitmap GetImageBmp();
        }
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
        public interface ICardSummon:ICardAssist
        {
            ElementType GetElementType();
            int GetDamage();
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
        public interface ICardCharacter : ICardBase
        {
            CharacterType GetCharacterType();//卡片角色类型
            int GetMaxHP();//最大生命值
            int GetMaxMP();//最大充能
            void GameStartAction();//游戏开始时候的事件
            void OnStageAction();//切换上场的事件
            void OffStageAction();//切换下场的事件
            List<ISkill> GetSkills();//各种技能
        }

    }
}
//################################################################
//Unity制作的游戏本体实现这些接口
//与游戏流程有关
//制作卡牌时可以调用查询信息
//################################################################
namespace TCGGame
{
    public enum RoundStage
    { 
        Pre_Fight,

        Pre_Round,//回合开始阶段
        During_Round,
        After_Round,//回合结束阶段

        After_Fight
    }
    public interface IGameBase
    {

    }
    namespace GameRoundInterface
    {
        public interface IGameStage : IGameBase
        {
            RoundStage GetRoundStage();

        }
    }
    namespace GameCharacterInterface
    {
        public interface IGameCharacter : IGameBase
        {
            List<ICardCharacterInfo> GetOurCharactersInfo();//获取我方角色信息
            List<ICardAssistInfo> GetOurAssistInfo();//获取我方场上辅助牌信息
            int GetOurCardLeft();//获取我方剩余的牌数量
            List<ICardInfo> GetOurCard();//获取我方手上的卡牌

            //List<touzi> GetOurDices();//获取我方手上的骰子


            //List<ICardCharacterInfo> GetOurSummonInfo();//获取我方召唤物信息

            List<ICardCharacterInfo> GetTheirCharactersInfo();//获取对方角色信息

        }
    }
}
//################################################################
//Unity制作的游戏本体实现这些接口
//与打出的卡牌相关，用来描述已经上场的牌的详细信息
//制作卡牌时可以调用查询信息
//################################################################
namespace TCGInfo
{
    namespace InfoInterface
    {
        public interface IInfoBase { }
        public interface IEffectInfo : IInfoBase
        {
            string GetEffectName();//对于卡牌制作来说只需要读取effect的名字然后判断即可
        }
        public interface ICardInfo : IInfoBase
        {
            ICardBase GetCard();
        }
        public interface ICardAssistInfo : ICardInfo
        {
            int GetLeftTimes();
        }
        public interface ICardCharacterInfo : ICardInfo
        {
            int GetCurrHP();
            int GetCurrMP();
            ElementType GetCurrElement();
            ICardAssistInfo GetCurrWeapon();
            ICardAssistInfo GetCurrArtifact();
            List<IEffectInfo> GetCurrEffects();
        }
        public interface ICardSummonInfo : ICardInfo
        {
            int GetLeftTimes();
        }
    }
}

