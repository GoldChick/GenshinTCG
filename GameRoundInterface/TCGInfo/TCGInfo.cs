using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCard;

//################################################################
//Unity制作的游戏本体实现这些接口
//与打出的卡牌相关，用来描述已经详细信息
//制作卡牌时可以调用查询信息
//################################################################
namespace TCGInfo
{
    public interface IInfo
    {

    }
    public class IInfo<T>:IInfo
    {
        private T info;
        public IInfo(T info)
        {
            this.info = info;
        }

        T GetValue()
        {
            return info;
        }
    }

    namespace InfoInterface
    {

        public interface ICardInfo : IInfo
        {
            ICardBase GetCard();
        }
        public interface ICardAssistInfo : IInfo
        {
            int GetLeftTimes();
        }
    }
}
