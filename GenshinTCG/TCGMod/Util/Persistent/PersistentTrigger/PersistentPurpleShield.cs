using TCGBase;
using TCGCard;
using TCGGame;

namespace TCGMod
{
    /// <summary>
    /// only for Hurt_Add
    /// </summary>
    public class PersistentPurpleShield : IPersistentTrigger
    {
        private int _line;
        private int _protect;
        /// <param name="line">结算到此buff，伤害超过_line时才触发</param>
        /// <param name="protect">一次抵挡多少伤害</param>
        public PersistentPurpleShield(int line, int protect)
        {
            _line = line;
            _protect = protect;
        }
        public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (variable is DamageVariable dv)
            {
                if(dv.Element>=0)
                {
                    if (dv.BaseDamage+dv.DamageModifier>=_line)
                    {
                        dv.DamageModifier -= _protect;
                        persitent.AvailableTimes--;
                    }
                }
            }
        }
    }
}
