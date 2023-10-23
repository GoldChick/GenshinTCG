using TCGBase;
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
                    Characters = new[] { "genshin3_3:qq", "genshin3_3:yoimiya", "genshin3_3:ayaka" },
                    ActionCards = new[] { "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon" ,
                                                      "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪" ,
                                                      "genshin3_3:赌徒的耳环", "genshin3_3:赌徒的耳环", "genshin3_3:赌徒的耳环", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪" ,
                                                      "genshin3_3:xingtianzhizhao", "genshin3_3:xingtianzhizhao", "genshin3_3:xingtianzhizhao", "genshin3_3:xingtianzhizhao", "genshin3_3:xingtianzhizhao" ,
                                                      "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword" ,
                                                      "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome" },
                }
            };
        }
        public  void InitServerSetting(ServerSetting setting,int preset)
        {
            switch (preset)
            {
                case 1:
                    ClientSetting = new()
                    {
                        Name = "DefaultBuiltIn",
                        DefaultCardSet = new PlayerNetCardSet()
                        {
                            Characters = new[] { "genshin3_3:nahida", "genshin3_3:qiqi", "genshin3_3:yaemiko" },
                            ActionCards = new[] { "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon" ,
                                                      "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪" ,
                                                      "genshin3_3:赌徒的耳环", "genshin3_3:赌徒的耳环", "genshin3_3:赌徒的耳环", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪" ,
                                                      "genshin3_3:xingtianzhizhao", "genshin3_3:xingtianzhizhao", "genshin3_3:xingtianzhizhao", "genshin3_3:xingtianzhizhao", "genshin3_3:xingtianzhizhao" ,
                                                      "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword" ,
                                                      "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome" },
                        }
                    };
                    break;
                default:
                    ClientSetting = new()
                    {
                        Name = "DefaultBuiltIn",
                        DefaultCardSet = new PlayerNetCardSet()
                        {
                            Characters = new[] { "genshin3_3:xiangling", "genshin3_3:mona", "genshin3_3:keqing" },
                            ActionCards = new[] { "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon", "genshin3_3:paimon" ,
                                                      "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪" ,
                                                      "genshin3_3:赌徒的耳环", "genshin3_3:赌徒的耳环", "genshin3_3:赌徒的耳环", "genshin3_3:参量质变仪", "genshin3_3:参量质变仪" ,
                                                      "genshin3_3:xingtianzhizhao", "genshin3_3:xingtianzhizhao", "genshin3_3:xingtianzhizhao", "genshin3_3:xingtianzhizhao", "genshin3_3:xingtianzhizhao" ,
                                                      "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword", "genshin3_3:sacrificial_sword" ,
                                                      "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome", "genshin3_3:leaveittome" },
                        }
                    };
                    break;
            }

        }
        public override void UpdateTeam(PlayerTeam me, PlayerTeam enemy)
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
