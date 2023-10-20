using TCGBase;
using TCGCard;
using TCGGame;
using TCGMod;
using TCGUtil;

namespace Genshin3_3
{
    public class 雷楔 : AbstractCardAction
    {
        public override string NameID => "雷楔";

        public override string[] SpecialTags => new string[] { "战斗行动" };

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
        public class 雷楔_Effect : AbstractCardPersistentEffect
        {
            public override bool Visible => false;

            public override int MaxUseTimes => 1;

            public override string NameID => "雷楔_effect";

            public override Dictionary<string, PersistentTrigger> TriggerDic => new()
            {
                { Tags.SenderTags.ROUND_ME_START,new((me,p,s,v)=>me.Game.HandleEvent(new(new(ActionType.UseSKill, 1)), me.TeamIndex)) },
                { Tags.SenderTags.AFTER_USE_SKILL, new((me,p,s,v)=>p.AvailableTimes--)}
            };
        }
    }

    public class Keqing : AbstractCardCharacter
    {
        public override int MaxMP => 3;

        public override AbstractCardSkill[] Skills => new AbstractCardSkill[] {
            new CharacterSimpleA("云来剑法",0,2,4),
            new XingDouGuiWei(),
            new TianJieXunYou() };

        public override string NameID => "keqing";

        public override ElementCategory CharacterElement => ElementCategory.ELECTRO;

        public override CharacterCategory CharacterCategory => CharacterCategory.HUMAN;

        public override CharacterRegion CharacterRegion => CharacterRegion.LIYUE;

        public override WeaponCategory WeaponCategory => throw new NotImplementedException();

        public class XingDouGuiWei : AbstractCardSkill
        {
            public override string NameID => "星斗归位";
            public override int[] Costs => new int[] { 0, 0, 0, 0, 1 };
            public override bool CostSame => false;

            public override SkillCategory Category => SkillCategory.E;

            public override void AfterUseAction(AbstractTeam me, Character c, int[]? targetArgs = null)
            {
                me.Enemy.Hurt(new(4, 3, 0), this);
                Logger.Warning("刻请使用了星斗归位！并且产生了一张雷楔！");
                if (me is PlayerTeam pt)
                {
                    var cih = pt.CardsInHand.Find(p => p.Card.NameID == "雷楔");
                    if (cih != null)
                    {
                        pt.CardsInHand.Remove(cih);
                        pt.AddPersistent(new Enchant("雷楔_enchant", 4, 2));
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
        }
        public class TianJieXunYou : AbstractCardSkill
        {
            public override string NameID => "天街巡游";
            public override int[] Costs => new int[] { 0, 0, 0, 0, 1 };
            public override bool CostSame => false;
            public override SkillCategory Category => SkillCategory.Q;

            public override void AfterUseAction(AbstractTeam me, Character c, int[]? targetArgs = null)
            {
                me.Enemy.MultiHurt(new DamageVariable[]
                {
                new(4, 4,  0) ,
                new(-1, 3,  0, true)
                }, this);
                Logger.Warning("刻请使用了天街巡游！");
            }

        }
    }
}
