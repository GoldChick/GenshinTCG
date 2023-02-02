using TCGCard;

namespace TCGBase
{
    public interface IDamage
    {
        int GetValue();
        bool DoPierce();
        ElementType GetElementType();
        ICardCharacter GetTarget();
        ICardBase GetSource();
    }
}
