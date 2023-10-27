using System;
using TCGBase;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class XingTianZhiZhao : AbstractCardEvent
    {
        public override string NameID => "xingtianzhizhao";

        public override bool CostSame => false;

        public override string[] SpecialTags => Array.Empty<string>();

        public override int[] Costs => Array.Empty<int>();


        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            Logger.Error("使用了星天之兆!");
            me.Characters[me.CurrCharacter].MP++;
        }

        public override bool CanBeUsed(PlayerTeam me, int[]? targetArgs = null)
        {
            var c = me.Characters[me.CurrCharacter];
            return c.MP < c.Card.MaxMP;
        }

    }
}
