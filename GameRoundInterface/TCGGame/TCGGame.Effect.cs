using System;
using System.Collections.Generic;
using TCGBase;
using TCGCard;
using TCGInfo;

namespace TCGGame
{
    public class Effect
    {
        public ICardEffect effect;//对应的卡牌资源

        public int useTimes;
        public bool visible;

        public Effect(ICardEffect effect, bool visible)
        {
            this.effect = effect;
            this.visible = visible;

            Reset();
        }
        public void Reset()
        {
            useTimes = effect.MaxUseTimes;
        }
        public static void Trigger(EffectTriggerType triggerType, Action<Effect> action, params List<Effect>[] multiple_effects)
        {
            foreach (List<Effect> effects in multiple_effects)
            {
                foreach (Effect effect in effects)
                {
                    if (effect.CanWork(triggerType))
                    {
                        action(effect);
                    }
                }
            }
        }
        public bool CanWork(EffectTriggerType triggerType)
        {
            // return ((int)effect.TriggerType & (int)triggerType) == (int)triggerType;
            return true;
        }
    }
}
