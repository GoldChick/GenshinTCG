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
            useTimes = effect.GetMaxUseTimes();
        }
    }
}
