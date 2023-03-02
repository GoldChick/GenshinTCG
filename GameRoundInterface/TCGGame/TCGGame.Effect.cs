using System;
using System.Collections.Generic;
using TCGBase;
using TCGCard;

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
        /// <summary>
        /// 不同的effect需要的参数可能不同
        /// 在action里预制
        /// </summary>
        public static void Trigger(EffectTriggerType triggerType, ActionType actionType, Action<Effect> action, params List<Effect>[] multiple_effects)
        {
            foreach (List<Effect> effects in multiple_effects)
            {
                foreach (Effect effect in effects)
                {
                    if (effect.CanWork(triggerType, actionType))
                    {
                        action(effect);
                    }
                }
            }
        }
        public bool CanWork(EffectTriggerType triggerType, ActionType actionType)
        {
            // return ((int)effect.TriggerType & (int)triggerType) == (int)triggerType;
            return true;
        }
    }
}
