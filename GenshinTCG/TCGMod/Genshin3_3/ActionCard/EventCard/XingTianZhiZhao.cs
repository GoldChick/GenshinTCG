using System;
using TCGBase;
using TCGCard;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class XingTianZhiZhao : ICardEvent
    {
        public int MaxNumPermitted => 2;

        public string NameID => "xingtianzhizhao";

        public bool CostSame => false;

        public string[] Tags => Array.Empty<string>();

        public int[] Costs => Array.Empty<int>();


        public void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            Logger.Error("使用了星天之兆!");
            me.Characters[me.CurrCharacter].MP++;
        }

        public bool CanBeUsed(PlayerTeam me, int[]? targetArgs = null)
        {
            var c = me.Characters[me.CurrCharacter];
            return c.MP < c.Card.MaxMP;
        }

    }
}
