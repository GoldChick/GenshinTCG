using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGBase;
using TCGCard;
using TCGInfo.InfoInterface;

namespace TCGInfo
{
    public interface ICardCharacterInfo : ICardInfo
    {
        int GetCurrHP();
        int GetCurrMP();
        ElementType GetCurrElement();
        ICardAssistInfo GetCurrWeapon();
        ICardAssistInfo GetCurrArtifact();
        List<IEffectInfo> GetCurrEffects();
    }
}
