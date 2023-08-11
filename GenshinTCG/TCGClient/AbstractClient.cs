using TCGAI;
using TCGGame;
using TCGUtil;
using TCGBase;
namespace TCGClient
{
    public abstract class AbstractClient
    {
        public ClientSetting ClientSetting { get; init; }
        public ServerSetting ServerSetting { get; protected set; }

        public AbstractTeam Me { get; protected set; }
        public AbstractTeam Enemy { get; protected set; }

        public abstract AbstractCardSet RequestCardSet();

        public abstract AIEvent RequestEvent(AIEventType demand,string help_txt="Null");

        public abstract void InitServerSetting(ServerSetting setting);

        public abstract void UpdateTeam(AbstractTeam me,AbstractTeam enemy);
    }
}
