using TCGBase;
using TCGGame;

namespace TCGCard
{
    /// <summary>
    /// 可以使用/打出的东西
    /// </summary>
    public interface IUsable : ICost
    {
        /// <summary>
        /// 打出后发生什么
        /// </summary>
        public void AfterUseAction(AbstractGame game, int meIndex);
    }
}
