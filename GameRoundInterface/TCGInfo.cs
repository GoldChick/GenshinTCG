using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCard.CardInterface;
using TCGCard;

//################################################################
//Unity制作的游戏本体实现这些接口
//与打出的卡牌相关，用来描述已经上场的牌的详细信息
//制作卡牌时可以调用查询信息
//################################################################
namespace TCGInfo
{
    namespace InfoInterface
    {
        public interface IInfoBase   {  }

        public interface ICardInfo : IInfoBase
        {
            ICardBase GetCard();
        }
        public interface ICardAssistInfo : ICardInfo
        {
            int GetLeftTimes();
        }

        public interface ICardSummonInfo : ICardInfo
        {
            int GetLeftTimes();
        }
    }
}
