using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCGCard
{
    public enum EffectType
    {
        Character,
        Team
    }
    public interface IEffect
    {
        string GetEffectName();
        EffectType GetEffectType();
        int GetMaxUseTimes();
        void BeHurtEvent();
        void AttackEvent();
        
    }
}
