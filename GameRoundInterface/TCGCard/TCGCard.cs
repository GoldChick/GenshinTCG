using System.Collections.Generic;
using System.Drawing;
using TCGGame;
//################################################################
//仅用于客户端使用
//不含有任何游戏机制
//################################################################
namespace TCGCard
{
    public interface IUtils
    {
        public string GetLibName();
        public string GetLibVersion();
        public string GetLibDescription();
        public List<string> GetDepentdencies();
    }
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

        Summon,

        Effect,
        Skill

    }

    public interface ICardBase
    {
        public string Name { get; }
        public string Description { get; }
        public CardType CardType { get; }
        Bitmap GetImageBmp();
    }
}



