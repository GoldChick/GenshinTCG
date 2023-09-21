using TCGBase;
using TCGCard;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class Keqing : ICardCharacter
    {
        public string MainElement => TCGBase.Tags.ElementTags.ELECTRO;

        public int MaxHP => 10;

        public int MaxMP => 3;

        public IEffect? DefaultEffect => null;

        public ICardSkill[] Skills => new ICardSkill[] { new YunLaiSword(), new XingDouGuiWei() };

        public string NameID => "keqing";

        public string[] Tags => new string[] { TCGBase.Tags.CardTags.RegionTags.LIYUE,
         TCGBase.Tags.CardTags.CharacterTags.HUMAN, TCGBase.Tags.ElementTags.ELECTRO
        };
        public class YunLaiSword : ICardSkill
        {
            public string NameID => "yunlai_sword";

            public string[] Tags => new string[] { TCGBase.Tags.SkillTags.E };

            public int[] Costs => new int[] { 1 };
            //TODO: for test: any dice
            public bool CostSame => false;

            public void AfterUseAction(AbstractGame game, int meIndex)
            {
                game.Teams[1 - meIndex].Hurt(0, 2, DamageSource.Character, 0);
                Logger.Warning("刻请使用了原来剑法");
            }

            public bool CanBeUsed(AbstractGame game, int meIndex) => true;
        }
        public class XingDouGuiWei : ICardSkill
        {
            public string NameID => "xingdouguiwei";
            public string[] Tags => new string[] { TCGBase.Tags.SkillTags.E };
            public int[] Costs => new int[] { 0, 0, 0, 0, 1 };
            public bool CostSame => false;

            public void AfterUseAction(AbstractGame game, int meIndex)
            {
                game.Teams[1 - meIndex].Hurt(0, 3, DamageSource.Character, 0);
                Logger.Warning("刻请使用了星斗归位！并且产生了一张雷楔！(暂时用交给我吧代替)");
                var me = game.Teams[meIndex];
                if (me is PlayerTeam pt)
                {
                    //TODO:生成雷楔
                    pt.GainCard(new LeaveItToMe());
                }
            }

            public bool CanBeUsed(AbstractGame game, int meIndex) => true;
        }
    }
}
