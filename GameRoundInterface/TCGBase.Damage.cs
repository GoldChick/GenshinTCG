using TCGCard;

namespace TCGBase
{
    public interface IDamage
    {
        int GetValue();
        bool DoPierce();
        ElementType GetElementType();
        int GetTargetId();
        ICardBase GetSource();
    }
}
