using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGInfo;
using TCGInfo.InfoInterface;

namespace TCGGame
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
