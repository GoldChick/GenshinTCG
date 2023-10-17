using TCGBase;
using TCGCard;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class 赌徒 : ICardArtifact, ITargetSelector
    {
        public override string NameID => "赌徒的耳环";

        public override string[] SpecialTags => new string[] { TCGBase.Tags.CardTags.EquipmentTags.ARTIFACT };

        public override int[] Costs => new int[] { 1 };

        public override bool CostSame => false;

        public TargetEnum[] TargetEnums => new TargetEnum[] { TargetEnum.Character_Me };

        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            Logger.Print($"对着{targetArgs[0]}号角色使用了赌徒的耳环!");
        }

        public override bool CanBeUsed(PlayerTeam me, int[]? targetArgs = null) => true;
        public class 赌徒_Effect : AbstractCardPersistentEffect
        {
            public override int MaxUseTimes => 3;

            public override Dictionary<string, PersistentTrigger> TriggerDic => throw new NotImplementedException();

            public override string NameID => "赌徒_effect";

            private class 赌徒_Trigger : PersistentTrigger
            {
                public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
