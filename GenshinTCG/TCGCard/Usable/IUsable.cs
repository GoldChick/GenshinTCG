using TCGBase;
using TCGGame;

namespace TCGCard
{
    public interface IUsable : ICost
    {

    }
    public interface IUsable<T> : IUsable where T : AbstractTeam
    {
        public void AfterUseAction(T me, int[]? targetArgs = null);
    }
}
