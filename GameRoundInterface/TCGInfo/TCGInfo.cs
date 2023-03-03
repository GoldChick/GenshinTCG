using TCGCard;

//################################################################
//游戏本体实现这些接口
//与打出的卡牌相关，用来描述已经详细信息
//制作卡牌时可以调用查询信息
//################################################################
namespace TCGInfo
{
    public interface IInfo
    {

    }
    public class IInfo<T> : IInfo
    {
        public readonly T Info;
        public IInfo(T info)
        {
            Info = info;
        }
    }
}
