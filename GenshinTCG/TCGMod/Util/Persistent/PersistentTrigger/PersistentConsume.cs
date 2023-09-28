using TCGBase;
using TCGCard;
using TCGGame;

namespace TCGMod
{
    /// <summary>
    /// 可用次数--
    /// </summary>
    public class PersistentConsume : IPersistentTrigger
    {
        private Func<AbstractTeam, AbstractPersistent, AbstractSender, AbstractVariable?, bool>? _condition;
        public PersistentConsume(Func<AbstractTeam, AbstractPersistent, AbstractSender, AbstractVariable?, bool>? condition = null)
        {
            _condition = condition;
        }
        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (_condition == null || _condition.Invoke(me, persitent, sender, variable))
            {
                persitent.AvailableTimes--;
            }
        }
    }
}
