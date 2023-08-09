using TCGAI;

namespace TCGGame
{
    public abstract class AbstractGame
    {
        public AbstractTeam[] Teams { get; init; }
        internal AIConnector[] AIConnectors { get; } = { null, null };
        public void EffectAct()
        {

        }
        public void Step()
        {

        }
        public string GetState()
        {
            return null;
        }
    }
}
