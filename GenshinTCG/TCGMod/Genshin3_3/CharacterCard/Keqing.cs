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

        public ICardSkill[] Skills => new ICardSkill[] { new YunLaiSword() };

        public string NameID => "keqing";

        public string[] Tags => throw new NotImplementedException();
        public class YunLaiSword : ICardSkill
        {
            public string NameID => "yunlai_sword";

            public string[] Tags => throw new NotImplementedException();

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
    }
}
