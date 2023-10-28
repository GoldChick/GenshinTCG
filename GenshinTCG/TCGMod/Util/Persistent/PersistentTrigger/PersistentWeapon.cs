using TCGBase;

namespace TCGMod
{
    public class PersistentWeapon: PersistentTrigger
    {
        public override void Trigger(PlayerTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
        {
            if (PersistentFunc.IsCurrCharacterDamage(me,persitent,sender,variable,out var dv))
            {
                dv.Damage++;
            }
        }
    }
}
