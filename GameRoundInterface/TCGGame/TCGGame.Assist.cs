using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCard;
//################################################################
//在手中的牌
//################################################################
namespace TCGGame
{
    public class Assist
    {
        /// <summary>
        /// 加载卡牌的时候分配的Id
        /// 可以用这个给卡牌排序
        /// </summary>
        public int cardId;
        public ICardAssist cardAssist;
        public Assist(int cardId, ICardAssist cardAssist)
        {
            this.cardId = cardId;
            this.cardAssist = cardAssist;
        }
    }
}
