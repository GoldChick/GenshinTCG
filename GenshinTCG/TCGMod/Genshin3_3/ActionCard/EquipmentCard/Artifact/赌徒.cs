using TCGBase;
using TCGCard;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class 赌徒 : ICardArtifact, ITargetSelector
    {
        public int MaxNumPermitted => 2;

        public string NameID => "赌徒的耳环";

        public string[] Tags => new string[] { TCGBase.Tags.CardTags.EquipmentTags.ARTIFACT };

        public int[] Costs => new int[] { 1 };

        public bool CostSame => false;

        public TargetEnum[] TargetEnums => new TargetEnum[] { TargetEnum.Character_Me };

        public void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            Logger.Print($"对着{targetArgs[0]}号角色使用了赌徒的耳环!");
        }

        public bool CanBeUsed(PlayerTeam me, int[]? targetArgs = null) => true;
        public class 赌徒_Effect : IEffect
        {
            public bool Visible => true;

            public bool Stackable => true;

            public bool DeleteWhenUsedUp => false;

            public int MaxUseTimes => 3;

            public Dictionary<string, IPersistentTrigger> TriggerDic => throw new NotImplementedException();

            public string NameID => "赌徒_effect";

            public string[] Tags => new string[] { };
            private class 赌徒_Trigger : IPersistentTrigger
            {
                public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
