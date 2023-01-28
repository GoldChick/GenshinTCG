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
    public enum RoundAction
    {
        Pass,
        UseSkill,
        PrepareSkill,
        ChangeCharacter
    }
    public interface IGameBase
    {

    }
}
