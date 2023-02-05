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


    public interface ICardBase
    {
        string GetCardName();
        string GetCardDiscription();
        CardType GetCardType();
        Bitmap GetImageBmp();
    }
}



