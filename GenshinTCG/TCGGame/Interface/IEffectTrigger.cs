using TCGBase;
using TCGGame;

namespace GenshinTCG.TCGGame.Interface
{
    public interface IEffectTrigger
    {
        public void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable);
    }
}
