using TCGBase;
using TCGCard;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class SacrificialSword : ICardWeapon, ITargetSelector
    {
        public string WeaponType => TCGBase.Tags.CardTags.WeaponTags.SWORD;

        public int MaxNumPermitted => 2;

        public string NameID => "sacrificial_sword";

        public string[] Tags => new string[] { WeaponType, TCGBase.Tags.CardTags.EquipmentTags.WEAPON };

        public int[] Costs => new int[] { 1 };

        public bool CostSame => false;

        public TargetEnum[] TargetEnums => new TargetEnum[] { TargetEnum.Character_Me };


        public void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            Logger.Print($"给{targetArgs[0]}号角色装备了一张祭礼剑！");
            me.Characters[targetArgs[0]].Weapon = new Effect(new 祭礼剑_effect());
        }

        public bool CanBeUsed(PlayerTeam me, int[]? targetArgs = null)
                => me.Characters[targetArgs[0]].Card.Tags.Contains(TCGBase.Tags.CardTags.WeaponTags.SWORD);
        public class 祭礼剑_effect : AbstractCardEffect
        {

            public override int MaxUseTimes => 1;

            public override string NameID => "祭礼剑_effect";

            public override Dictionary<string, IPersistentTrigger> TriggerDic => new() { { TCGBase.Tags.SenderTags.AFTER_USE_SKILL, new 祭礼剑_Trigger() } };

            private class 祭礼剑_Trigger : IPersistentTrigger
            {
                public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
                {
                    if (sender is UseSkillSender sks && sks.Skill.Tags.Contains(TCGBase.Tags.SkillTags.E))
                    {
                        Logger.Error("使用了E，触发了祭礼剑的效果!");
                    }
                }
            }


        }
    }
}
