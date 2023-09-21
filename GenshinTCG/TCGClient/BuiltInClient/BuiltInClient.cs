﻿using TCGBase;
using TCGGame;
using TCGUtil;

namespace TCGClient
{
    /// <summary>
    /// 和服务端共生的AI客户端
    /// 只用于playerteam
    /// </summary>
    internal partial class BuiltInClient : AbstractClient
    {
        public PlayerTeam MePt { get; protected set; }
        //内置客户端无冷却
        public override NetEvent RequestEvent(ActionType demand, string help_txt = "Null")
        {
            //TODO 没写完
            Console.WriteLine($"AI Demand For %:{help_txt}");
            return Act(demand);
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
            if(me is PlayerTeam pt)
            {
                Me = me;
                MePt = pt;
            }
            else
            {
                throw new ArgumentException("BuiltInClient:TeamMe不是PlayerTeam!");
            }
            Enemy = enemy;
        }
        public override AbstractServerCardSet RequestCardSet()
        {
            return new ServerPlayerCardSet(ClientSetting.DefaultCardSet as PlayerNetCardSet);
        }
    }
}
