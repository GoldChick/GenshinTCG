namespace TCGBase
{
    /// <summary>
    /// only for Hurt_Add
    /// </summary>
    public class PersistentPurpleShield : PersistentTrigger
    {
        private readonly int _line;
        private readonly int _protect;
        /// <param name="line">结算到此buff，伤害大于等于_line时才触发</param>
        /// <param name="protectNum">一次抵挡多少伤害</param>
        public PersistentPurpleShield(int protectNum, int line = 1)
        {
            _line = line;
            _protect = protectNum;
        }
        public override void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (persitent.AvailableTimes > 0 && variable is DamageVariable dv && sender.TeamID == me.TeamIndex)
            {
                if (persitent.PersistentRegion < 0 || persitent.PersistentRegion > 10 || me.CurrCharacter == persitent.PersistentRegion)
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
