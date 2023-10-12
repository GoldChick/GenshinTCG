using TCGBase;
using TCGCard;
using TCGGame;
using TCGUtil;

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
            if (persitent.AvailableTimes > 0 && variable is DamageVariable dv && sender.TeamID == me.TeamIndex)
            {
                if (persitent is not PersonalEffect pe || me.CurrCharacter == pe.Owner)
                {
                    if (dv.Element >= 0)
                    {
                        if (dv.Damage >= _line)
                        {
                            dv.Damage -= _protect;
                            persitent.AvailableTimes--;
                        }
                    }
                }
            }
        }
    }
}
