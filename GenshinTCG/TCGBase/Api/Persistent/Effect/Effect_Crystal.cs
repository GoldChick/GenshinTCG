using TCGBase;

namespace Minecraft
{
    public class Effect_Crystal : AbstractCardEffect
    {
        public override int InitialUseTimes => 1;
        public override int MaxUseTimes => 2;

        public override PersistentTriggerDictionary TriggerDic => new()
        {
            new PersistentPreset.HurtDecreaseYellowShield()
        };
    }
}
