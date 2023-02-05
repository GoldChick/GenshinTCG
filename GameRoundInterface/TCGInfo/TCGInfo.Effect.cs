using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCard;
using TCGInfo.InfoInterface;

namespace TCGInfo
{
    public interface IEffectInfo : IInfoBase
    {
        string GetEffectName();//对于卡牌制作来说只需要读取effect的名字然后判断即可
        EffectType GetEffectType();
        int GetLeftTimes();
    }
}
