using System;

namespace TCGBase
{
    public partial class AbstractTeam
    {
        public virtual void DoDamage(DamageVariable dv, IDamageSource ds, Action? action = null, Action? seperateAction = null)
        {

        }
    }
}
