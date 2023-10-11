﻿using TCGBase;
using TCGCard;
using TCGGame;
using TCGMod;
using TCGUtil;

namespace Genshin3_3
{
    public class 雷楔 : AbstractCardAction
    {
        public override string NameID => "雷楔";

        public override string[] Tags => new string[] { "战斗行动" };

        public override int[] Costs => new int[] { 0, 0, 0, 0, 1 };

        public override bool CostSame => false;


        public override void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            Logger.Error("打出了一张雷楔！但是什么都没有发生！");
            for (int i = 0; i < me.Characters.Length; i++)
            {
                if (me.Characters[i].Card.NameID == "keqing")
                {
                    if (i != me.CurrCharacter)
                    {
                        me.AddPersistent(new 雷楔_Effect());
                        me.Game.HandleEvent(new(new(ActionType.SwitchForced, i)), me.TeamIndex);
                    }
                    break;
                }
            }
        }
        public class 雷楔_Effect : AbstractCardEffect
        {
            public override bool Visible => false;

            public override int MaxUseTimes => 1;

            public override string NameID => "雷楔_effect";

            public override Dictionary<string, IPersistentTrigger> TriggerDic => new() {
                { TCGBase.Tags.SenderTags.ROUND_ME_START,new 雷楔_Trigger() },
                { TCGBase.Tags.SenderTags.AFTER_USE_SKILL, new PersistentSimpleConsume() }
            };
            private class 雷楔_Trigger : IPersistentTrigger
            {
                public void Trigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
                {
                    Logger.Print("雷楔effect发力了！");
                    me.Game.HandleEvent(new(new(ActionType.UseSKill, 1)), me.TeamIndex);
                }
            }


        }
    }

    public class Keqing : AbstractCardCharacter
    {
        public override string MainElement => TCGBase.Tags.ElementTags.ELECTRO;

        public override int MaxMP => 3;

        public override AbstractCardSkill[] Skills => new AbstractCardSkill[] { new YunLaiSword(), new XingDouGuiWei(), new TianJieXunYou() };

        public override string NameID => "keqing";

        public override string[] Tags => new string[] { TCGBase.Tags.CardTags.RegionTags.LIYUE,
         TCGBase.Tags.CardTags.CharacterTags.HUMAN, TCGBase.Tags.ElementTags.ELECTRO,
         TCGBase.Tags.CardTags.WeaponTags.SWORD
        };
        public class YunLaiSword : AbstractCardSkill
        {
            public override string NameID => "yunlai_sword";

            public override string[] Tags => new string[] { TCGBase.Tags.SkillTags.NORMAL_ATTACK };

            public override int[] Costs => new int[] { 1 };
            public override bool CostSame => false;

            public override void AfterUseAction(AbstractTeam me, int[]? targetArgs = null)
            {
                me.Enemy.Hurt(new(0, 2, DamageSource.Character, 0));
                Logger.Warning("刻请使用了原来剑法，为了测试方便为水元素伤害");
            }

        }
        public class XingDouGuiWei : AbstractCardSkill
        {
            public override string NameID => "xingdouguiwei";
            public override string[] Tags => new string[] { TCGBase.Tags.SkillTags.E };
            public override int[] Costs => new int[] { 0, 0, 0, 0, 1 };
            public override bool CostSame => false;

            public override void AfterUseAction(AbstractTeam me, int[]? targetArgs = null)
            {
                me.Enemy.Hurt(new(4, 3, DamageSource.Character, 0));
                Logger.Warning("刻请使用了星斗归位！并且产生了一张雷楔！");
                if (me is PlayerTeam pt)
                {
                    var c = pt.CardsInHand.Find(p => p.Card.NameID == "雷楔");
                    if (c != null)
                    {
                        pt.CardsInHand.Remove(c);
                        pt.AddPersistent(new 雷楔_Enchant());
                    }
                    else if (me.Effects.Contains("雷楔_effect"))
                    {
                        Logger.Warning("由于使用雷楔出来，所以不会产生雷楔，！");
                    }
                    else
                    {
                        pt.GainCard(new 雷楔());
                    }
                }
            }
            public class 雷楔_Enchant : AbstractCardEffect
            {
                //使用雷楔后的雷附魔
                public override int MaxUseTimes => 2;
                public override string NameID => "雷楔_enchant";
                public override Dictionary<string, IPersistentTrigger> TriggerDic => new() {
                { TCGBase.Tags.SenderTags.ROUND_OVER,new PersistentSimpleConsume() },
                { TCGBase.Tags.SenderTags.ELEMENT_ENCHANT, new PersistentElementEnchant(4) }
            };
            }
        }
        public class TianJieXunYou : AbstractCardSkill
        {
            public override string NameID => "天街巡游";
            public override string[] Tags => new string[] { TCGBase.Tags.SkillTags.Q };
            public override int[] Costs => new int[] { 0, 0, 0, 0, 1 };
            public override bool CostSame => false;

            public override void AfterUseAction(AbstractTeam me, int[]? targetArgs = null)
            {
                me.Enemy.MultiHurt(new DamageVariable[]
                {
                new(4, 4, DamageSource.Character, 0) ,
                new(-1, 3, DamageSource.Character, 0,true)
                });
                Logger.Warning("刻请使用了天街巡游！");
            }

        }
    }
}
