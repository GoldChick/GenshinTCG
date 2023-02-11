using TCGGame;
//################################################################
//注意：这里的Assist卡打出后需要在对应区域召唤Effect卡才能生效
//################################################################
namespace TCGCard
{
    public enum CardAssistType
    {
        Nature,
        Weapon,
        Artifact,
        Place,
        Food,
        Event,
        Summon
    }
    public interface ICardAssist : ICardBase
    {
        public int MaxNumPermitted { get; }//允许携带的最大数量
        public int MaxUseTimes { get; }
        public bool CanBeMultiplyUsed { get; }//是否能够多次使用，即每回合开始阶段刷新。否则次数耗尽后消失
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

}
