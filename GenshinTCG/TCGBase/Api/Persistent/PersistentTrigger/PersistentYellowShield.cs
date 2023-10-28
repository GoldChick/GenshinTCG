namespace TCGBase
{
    /// <summary>
    /// only for Hurt_Add
    /// </summary>
    public class PersistentYellowShield : PersistentTrigger
    {
        public override void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (persitent.AvailableTimes > 0 && variable is DamageVariable dv && sender.TeamID == me.TeamIndex)
            {
                if (persitent is not PersonalEffect || me.CurrCharacter == persitent.PersistentRegion)
                {
                    if (dv.Element >= 0)
                    {
                        int a = int.Min(persitent.AvailableTimes, dv.Damage);
                        dv.Damage -= a;
                        persitent.AvailableTimes -= a;
                    }
                }
            }
        }
    }
}
