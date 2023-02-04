using System.Collections.Generic;
using TCGCard;
using TCGGame;

namespace TCGBase
{
    public interface IDamage
    {
        int GetValue();
        bool DoPierce();
        List<Effect> GetEffects();
        ElementType GetElementType();
        int GetTargetId();
        ICardBase GetSource();
    }
}
