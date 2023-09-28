using TCGBase;
using TCGCard;
using TCGGame;

namespace TCGMod
{
    /// <summary>
    /// only for Hurt_Add
    /// </summary>
    public class PersistentYellowShield : IPersistentTrigger
    {
        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (variable is DamageVariable dv)
            {
                if (dv.Element >= 0)
                {
                    int a = int.Min(persitent.AvailableTimes, dv.Damage );
                    dv.Damage -= a;
                    persitent.AvailableTimes -= a;
                }
            }
        }
    }
}
