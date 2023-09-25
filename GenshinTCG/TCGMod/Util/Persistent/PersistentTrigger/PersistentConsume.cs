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
        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            persitent.AvailableTimes--;
        }
    }
}
