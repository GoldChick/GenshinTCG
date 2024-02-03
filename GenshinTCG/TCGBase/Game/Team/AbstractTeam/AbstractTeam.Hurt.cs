using System;

namespace TCGBase
{
    public partial class AbstractTeam
    {
        public virtual void DoDamage(DamageVariable dv, ITriggerable triggerable, Action? action = null, Action? seperateAction = null)
        {

        }
    }
}
