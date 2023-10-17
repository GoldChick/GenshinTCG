using TCGBase;
using TCGCard;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class SacrificialSword : AbstractCardWeapon
    {
        public override string NameID => "sacrificial_sword";

        public override int[] Costs => new int[] { 1 };

        public override bool CostSame => false;

        public override WeaponCategory WeaponCategory => WeaponCategory.SWORD;

        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            Logger.Print($"给{targetArgs[0]}号角色装备了一张祭礼剑！");
            me.Characters[targetArgs[0]].Weapon = new Effect(new 祭礼剑_effect());
        }
        public class 祭礼剑_effect : AbstractCardPersistentEffect
        {

            public override int MaxUseTimes => 1;

            public override string NameID => "祭礼剑_effect";

            public override Dictionary<string, PersistentTrigger> TriggerDic => new() { { TCGBase.Tags.SenderTags.AFTER_USE_SKILL, new 祭礼剑_Trigger() } };

            private class 祭礼剑_Trigger : PersistentTrigger
            {
                public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
                {
                    if (sender is UseSkillSender sks && sks.Skill.SpecialTags.Contains(TCGBase.Tags.SkillTags.E))
                    {
                        Logger.Error("使用了E，触发了祭礼剑的效果!");
                    }
                }
            }


        }
    }
}
