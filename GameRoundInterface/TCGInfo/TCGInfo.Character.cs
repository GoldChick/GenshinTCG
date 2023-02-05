using System.Collections.Generic;
using TCGBase;
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
