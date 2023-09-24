using System;
using TCGBase;
using TCGCard;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class 雷楔 : ICardAction
    {
        public int MaxNumPermitted => 1;

        public string NameID => "雷楔";

        public string[] Tags => new string[] { "战斗行动" };

        public int[] Costs => new int[] { 0, 0, 0, 0, 1 };

        public bool CostSame => false;


        public void AfterUseAction(PlayerTeam me, int[]? targetArgs = null)
        {
            Logger.Error("打出了一张雷楔！但是什么都没有发生！");
            var tm = me as PlayerTeam;
            for (int i = 0; i < tm.Characters.Length; i++)
            {
                if (tm.Characters[i].Card.NameID == "keqing")
                {
                    if (i != tm.CurrCharacter)
                    {
                        me.AddPersistent(new 雷楔_Effect());
                        me.Game.HandleEvent(new(new(ActionType.SwitchForced, i)), me.TeamIndex);
                    }
                    break;
                }
            }
        }
        public bool CanBeUsed(PlayerTeam me, int[]? targetArgs = null) => true;

        public class 雷楔_Effect : IEffect
        {
            public bool Visible => false;

            public bool Stackable => false;

            public bool DeleteWhenUsedUp => true;

            public int MaxUseTimes => 1;

            public string NameID => "雷楔_effect";

            public string[] Tags => Array.Empty<string>();

            public void EffectTrigger(AbstractTeam me, AbstractPersistent persitent, AbstractSender sender, AbstractVariable? variable)
            {
                if (sender.SenderName == TCGBase.Tags.SenderTags.ROUND_ME_START)
                {
                    Logger.Print("雷楔effect发力了！");
                    me.Game.HandleEvent(new(new(ActionType.UseSKill, 1)), me.TeamIndex);
                }
                else if (sender.SenderName == TCGBase.Tags.SenderTags.AFTER_USE_SKILL)
                {
                    Logger.Print("雷楔effect:after_use_skill");
                    persitent.AvailableTimes--;
                }
            }
        }
    }

    public class Keqing : ICardCharacter
    {
        public string MainElement => TCGBase.Tags.ElementTags.ELECTRO;

        public int MaxHP => 10;

        public int MaxMP => 3;

        public IEffect? DefaultEffect => null;

        public ICardSkill[] Skills => new ICardSkill[] { new YunLaiSword(), new XingDouGuiWei() };

        public string NameID => "keqing";

        public string[] Tags => new string[] { TCGBase.Tags.CardTags.RegionTags.LIYUE,
         TCGBase.Tags.CardTags.CharacterTags.HUMAN, TCGBase.Tags.ElementTags.ELECTRO,
         TCGBase.Tags.CardTags.WeaponTags.SWORD
        };
        public class YunLaiSword : ICardSkill
        {
            public string NameID => "yunlai_sword";

            public string[] Tags => new string[] { TCGBase.Tags.SkillTags.NORMAL_ATTACK };

            public int[] Costs => new int[] { 1 };
            public bool CostSame => false;

            public void AfterUseAction(AbstractTeam me, int[]? targetArgs = null)
            {
                me.Enemy.Hurt(0, 2, DamageSource.Character, 0);
                Logger.Warning("刻请使用了原来剑法");
            }

        }
        public class XingDouGuiWei : ICardSkill
        {
            public string NameID => "xingdouguiwei";
            public string[] Tags => new string[] { TCGBase.Tags.SkillTags.E };
            public int[] Costs => new int[] { 0, 0, 0, 0, 1 };
            public bool CostSame => false;

            public void AfterUseAction(AbstractTeam me, int[]? targetArgs = null)
            {
                me.Enemy.Hurt(0, 3, DamageSource.Character, 0);
                Logger.Warning("刻请使用了星斗归位！并且产生了一张雷楔！");
                if (me is PlayerTeam pt)
                {
                    var c = pt.CardsInHand.Find(p => p.Card.NameID == "雷楔");
                    if (c != null)
                    {
                        pt.CardsInHand.Remove(c);
                    }
                    else if (me.Effects.Contains("雷楔_effect"))
                    {
                        Logger.Warning("由于使用雷楔出来，所以不会产生雷楔！");
                    }
                    else
                    {
                        pt.GainCard(new 雷楔());
                    }
                }
            }
        }
        public class TianJieXunYou : ICardSkill
        {
            public string NameID => "天街巡游";
            public string[] Tags => new string[] { TCGBase.Tags.SkillTags.E };
            public int[] Costs => new int[] { 0, 0, 0, 0, 1 };
            public bool CostSame => false;

            public void AfterUseAction(AbstractTeam me, int[]? targetArgs = null)
            {
                me.Enemy.Hurt(0, 3, DamageSource.Character, 0);
                Logger.Warning("刻请使用了天街巡游！");
            }

        }
    }
}
