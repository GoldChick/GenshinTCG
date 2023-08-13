using GenshinTCG.TCGGame.Interface;
using TCGBase;
using TCGCard;

namespace TCGGame
{
    public abstract class AbstractPersistent:IEffectTrigger
    {
        public abstract void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable);
    }
    public abstract class AbstractPersistent<T> : AbstractPersistent
    {
        public T Card { get; protected set; }
    }
}
