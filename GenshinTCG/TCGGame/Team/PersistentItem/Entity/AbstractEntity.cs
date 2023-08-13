using GenshinTCG.TCGGame.Interface;
using TCGBase;
using TCGUtil;
namespace TCGGame
{
    public enum EntityType
    {
        Support,
        Card,
        Character,
        Summon
    }
    public abstract class AbstractEntity : IEffectTrigger
    {
        public abstract EntityType Type { get; }

        public abstract void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable);
    }
}
