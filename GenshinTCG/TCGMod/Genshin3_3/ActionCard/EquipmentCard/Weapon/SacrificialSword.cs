using System.Reflection;
using TCGBase;
using TCGCard;
using TCGGame;
using TCGMod;
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
            //TODO:更替武器等
        }
        public class 祭礼剑_effect : AbstractCardPersistentEffect
        {
            public override int MaxUseTimes => 1;
            public override bool DeleteWhenUsedUp => false;
            public override string NameID => "祭礼剑_effect";

            public override PersistentTriggerDictionary TriggerDic => new()
            {
                {Tags.SenderTags.DAMAGE_INCREASE, new PersistentWeapon() },
                {Tags.SenderTags.AFTER_USE_SKILL, (me,p,s,v)=>
                    {
                        if (p.AvailableTimes > 0 && s is UseSkillSender sks && sks.Skill.Category == SkillCategory.E)
                        {
                            p.AvailableTimes--;
                        }
                    }
                },
                {Tags.SenderTags.ROUND_START, new PersistentSimpleUpdate() }
            };
        }
    }
}
