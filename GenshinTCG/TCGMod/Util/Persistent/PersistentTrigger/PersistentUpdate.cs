using TCGBase;
using TCGCard;
using TCGGame;

namespace TCGMod
{
    public class PersistentUpdate : IPersistentTrigger
    {
        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            persitent.AvailableTimes = int.Max(persitent.CardBase.MaxUseTimes, persitent.AvailableTimes);
        }
    }
}
