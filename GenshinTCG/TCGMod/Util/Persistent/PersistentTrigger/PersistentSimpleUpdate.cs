using TCGBase;
using TCGCard;
using TCGGame;

namespace TCGMod
{
    public class PersistentSimpleUpdate : IPersistentTrigger
    {
        private Func<AbstractTeam, AbstractPersistent, AbstractSender, AbstractVariable?, bool>? _condition;
        public PersistentSimpleUpdate(Func<AbstractTeam, AbstractPersistent, AbstractSender, AbstractVariable?, bool>? condition = null)
        {
            _condition = condition;
        }
        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (_condition == null || _condition.Invoke(me, persitent, sender, variable))
            {
                persitent.AvailableTimes = int.Max(persitent.CardBase.MaxUseTimes, persitent.AvailableTimes);
            }
        }
    }
}
