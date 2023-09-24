using TCGBase;
using TCGCard;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class Paimon : ICardSupport
    {
        public int MaxNumPermitted => 2;

        public string NameID => "paimon";

        public string[] Tags => new string[] { TCGBase.Tags.CardTags.AssistTags.PARTNER };

        public int[] Costs => new int[] { 3 };

        public bool CostSame => true;

        public void AfterUseAction(AbstractGame game, int meIndex)
        {
            Logger.Print("打出了一张大pi!");
            game.Teams[meIndex].AddPersistent(new PaimonSupport());
        }

        public bool CanBeUsed(AbstractGame game, int meIndex) => true;
        public class PaimonSupport : ISupport
        {
            public bool Stackable => true;

            public bool DeleteWhenUsedUp => true;

            public int MaxUseTimes => 2;

            public string NameID => "paimon_support";

            public string[] Tags => new string[] { TCGBase.Tags.CardTags.AssistTags.PARTNER };

            public void EffectTrigger(AbstractTeam me, AbstractTeam enemy, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
            {
                if (sender.SenderName == TCGBase.Tags.SenderTags.ROUND_START)
                {
                    persitent.AvailableTimes--;
                    Logger.Warning("大派触发了！");
                    if (me is PlayerTeam pt)
                    {
                        pt.AddDice(0);
                        pt.AddDice(0);
                    }
                    if (persitent.AvailableTimes == 0)
                    {
                        Logger.Warning("大派触发了！并且弃置！");
                    }
                }
            }
        }

    }
}
