using TCGBase;
using TCGGame;
using TCGUtil;

namespace TCGClient
{
    /// <summary>
    /// 和服务端共生的AI客户端
    /// </summary>
    internal partial class BuiltInClient : AbstractClient
    {
        //内置客户端无冷却
        public override Task<NetEvent> RequestEvent(ActionType demand, string help_txt = "Null")
        {
            //TODO 没写完
            Console.WriteLine($"AI Demand For %:{help_txt}");
            return new(() => Act(demand));
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


        public override AbstractServerCardSet RequestCardSet()
        {
            return new ServerPlayerCardSet(ClientSetting.DefaultCardSet as PlayerNetCardSet);
        }
    }
}
