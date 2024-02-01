using TCGBase;

namespace Minecraft
{
    public class Effect_Full : AbstractCardEffect
    {
        public override int MaxUseTimes => 1;

        public override PersistentTriggerList TriggerList => new()
        {
            { SenderTag.RoundOver,(me,p,s,v)=>p.AvailableTimes--}
        };
    }
}
