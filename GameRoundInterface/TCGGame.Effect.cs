using TCGBase;
using TCGCard;

namespace TCGGame
{
    public class Effect
    {
        public int useTimes;
        /// <summary>
        /// 从ActionType的枚举中搭配
        /// <para></para>
        /// 点击：<see cref="ActionType"/>
        /// </summary>
        public int actionType;
        public ICardEffect effect;//对应的卡牌资源

        public Effect(ICardEffect effect)
        {
            this.effect = effect;
            Reset();
        }
        public void Reset()
        {
            useTimes = effect.GetMaxUseTimes();
        }
    }
}
