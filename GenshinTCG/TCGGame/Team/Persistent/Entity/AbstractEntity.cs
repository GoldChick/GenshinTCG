using TCGUtil;
namespace TCGGame
{
    public enum EntityType
    {
        Assist,
        Card,
        Character,
        Summon
    }
    public abstract class AbstractEntity
    {
        public abstract EntityType Type { get; }
    }
}
