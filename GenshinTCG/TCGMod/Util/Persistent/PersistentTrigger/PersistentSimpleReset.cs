using TCGBase;
using TCGCard;
using TCGGame;

namespace TCGMod
{
    /// <summary>
    /// 把可用次数设为0
    /// </summary>
    public class PersistentSimpleReset : PersistentTrigger
    {
        public override void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            persitent.AvailableTimes = 0;
        }
    }
}
