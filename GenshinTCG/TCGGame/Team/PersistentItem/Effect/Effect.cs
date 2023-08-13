using TCGBase;
using TCGCard;

namespace TCGGame
{
    public class Effect : AbstractPersistent<IEffect>
    {
        public override void EffectTrigger(AbstractGame game, int meIndex, AbstractSender sender, AbstractVariable? variable)
        {
            Card.EffectTrigger(game.Teams[meIndex], game.Teams[1 - meIndex], sender, variable);
            game.Step();
        }
    }
}
