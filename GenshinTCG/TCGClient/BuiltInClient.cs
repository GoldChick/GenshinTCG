using TCGBase;
using TCGGame;
using TCGUtil;

namespace TCGClient
{
    /// <summary>
    /// 和服务端共生的AI客户端
    /// </summary>
    internal class BuiltInClient : AbstractClient
    {
        public async override Task<NetAction> RequestEvent(ActionType demand, string help_txt = "Null")
        {
            //TODO 没写完
            Console.WriteLine($"AI Demand For %:{help_txt}");
            return null;
        }

        public override void InitServerSetting(ServerSetting setting)
        {
            ClientSetting = new()
            {
                Name = "DefaultBuiltIn",
                DefaultCardSet = new PlayerNetCardSet()
                {
                    Characters = new[] { "genshin3_3:keqing", "genshin3_3:keqing", "genshin3_3:keqing" },
                    ActionCards = new[] { "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon" ,
                                                      "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon" ,
                                                      "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon" ,
                                                      "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon" ,
                                                      "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon" ,
                                                      "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon" },
                }
            };
        }

        public override void UpdateTeam(AbstractTeam me, AbstractTeam enemy)
        {
            throw new NotImplementedException();
        }

        public override AbstractServerCardSet RequestCardSet()
        {
            return new ServerPlayerCardSet(ClientSetting.DefaultCardSet as PlayerNetCardSet);
        }
    }
}
