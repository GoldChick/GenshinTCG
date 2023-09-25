using TCGBase;
using TCGCard;
using TCGGame;

namespace TCGMod
{
    /// <summary>
    /// 把可用次数设为0
    /// </summary>
    public class PersistentRemove : IPersistentTrigger
    {
        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            persitent.AvailableTimes = 0;
        }
    }
}
