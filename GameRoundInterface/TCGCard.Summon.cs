using TCGBase;
using TCGCard.CardInterface;

namespace TCGCard
{
    public interface ICardSummon : ICardAssist
    {
        ElementType GetElementType();
        int GetDamage();
    }
}
