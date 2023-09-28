using TCGCard;
using TCGGame;
using TCGUtil;

namespace Genshin3_3
{
    public class Mona : ICardCharacter
    {
        public string MainElement => TCGBase.Tags.ElementTags.HYDRO;

        public int MaxHP => 10;

        public int MaxMP => 3;

        public IEffect? DefaultEffect => null;

        public ICardSkill[] Skills => new[] { new 因果点破() };

        public string NameID => "mona";

        public string[] Tags => new string[] { TCGBase.Tags.CardTags.RegionTags.MONDSTADT,
        TCGBase.Tags.CardTags.WeaponTags.CATALYST,TCGBase.Tags.CardTags.CharacterTags.HUMAN};
        private class 因果点破 : ICardSkill
        {
            public string NameID => "yinguodianpo";

            public string[] Tags => new string[] { TCGBase.Tags.SkillTags.NORMAL_ATTACK };

            public int[] Costs => new int[] { 1 };

            public bool CostSame => false;

            public void AfterUseAction(AbstractTeam me, int[]? targetArgs = null)
            {
                Logger.Error("莫娜使用了因果点破(还没做)");
            }
        }
    }
}
