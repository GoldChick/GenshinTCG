using TCGAI;
using TCGBase;
using TCGGame;
using TCGUtil;

namespace TCGClient
{
    /// <summary>
    /// 和服务端不在一起编译，加载机制和mod类似
    /// </summary>
    internal class ModClient : AbstractClient
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

        public override AbstractServerCardSet RequestCardSet()
        {
            throw new NotImplementedException();
        }
    }
}
