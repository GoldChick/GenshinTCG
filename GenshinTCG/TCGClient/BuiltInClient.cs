using TCGAI;
using TCGBase;
using TCGGame;
using TCGUtil;

namespace TCGClient
{
    /// <summary>
    /// 和服务端共生的AI客户端
    /// </summary>
    public class BuiltInClient : AbstractClient
    {
        public override AIEvent RequestEvent(AIEventType demand, string help_txt = "Null")
        {
            throw new NotImplementedException();
        }

        public override void InitServerSetting(ServerSetting setting)
        {
            throw new NotImplementedException();
        }

        public override void UpdateTeam(AbstractTeam me, AbstractTeam enemy)
        {
            throw new NotImplementedException();
        }

        public override AbstractCardSet RequestCardSet()
        {
            throw new NotImplementedException();
        }
    }
}
