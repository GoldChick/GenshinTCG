using TCGGame;
using TCGInfo;
//################################################################
//注意：这里的Assist卡打出后需要在对应区域召唤Effect卡才能生效
//################################################################
namespace TCGCard
{
    public enum CardAssistType
    {
        Nature,
        Weapon,
        Artifact,
        Place,
        Food,
        Event,
        Summon
    }
}
